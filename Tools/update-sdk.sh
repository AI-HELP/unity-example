#!/bin/bash
# Update AIHelp SDK version pins in this Unity project (no Editor required).
#
# iOS is a Swift package (https://github.com/AI-HELP/aihelp-ios-sdk.git).
# Android is the Maven coordinate in mainTemplate.gradle.
#
#   bash Tools/update-sdk.sh --ios-version 6.3.0
#   bash Tools/update-sdk.sh --ios-version 6.3.0 --export
#   bash Tools/update-sdk.sh --android-only --android-version 6.3.+
#
# What it does:
#   1) Set IosPackageVersion in Assets/Plugins/Editor/Editor/PostProcessBuild.cs
#   2) Sync Android Maven line to <major.minor>.+ unless --android-version is set
#
# Options:
#   --ios-only / --android-only
#   --ios-version 6.3.0         # iOS tag; Android becomes major.minor.+
#   --android-version 6.3.+     # override the Maven version
#   --export                    # run Tools/export-unitypackage.sh after update
#   --dry-run
set -euo pipefail

PROJECT_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
POSTPROCESS="$PROJECT_ROOT/Assets/Plugins/Editor/Editor/PostProcessBuild.cs"
GRADLE="$PROJECT_ROOT/Assets/Plugins/Android/mainTemplate.gradle"
AAR_COORD="net.aihelp:android-aihelp-aar"

DO_IOS=1
DO_ANDROID=1
DO_EXPORT=0
DRY_RUN=0
IOS_VERSION=""
ANDROID_VERSION_OVERRIDE=""

usage() {
  sed -n '2,20p' "$0" | sed 's/^#[[:space:]]\{0,1\}//'
  exit "${1:-0}"
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    -h|--help) usage 0 ;;
    --ios-only) DO_ANDROID=0; shift ;;
    --android-only) DO_IOS=0; shift ;;
    --export) DO_EXPORT=1; shift ;;
    --dry-run) DRY_RUN=1; shift ;;
    --ios-version) IOS_VERSION="${2:-}"; shift 2 ;;
    --android-version) ANDROID_VERSION_OVERRIDE="${2:-}"; shift 2 ;;
    -*)
      echo "Unknown option: $1" >&2
      usage 1
      ;;
    *)
      echo "Unexpected arg: $1" >&2
      echo "iOS is a Swift package. Pass --ios-version <tag>." >&2
      usage 1
      ;;
  esac
done

if [[ "$DO_IOS" -eq 1 && -z "$IOS_VERSION" && -z "$ANDROID_VERSION_OVERRIDE" ]]; then
  echo "Usage: bash Tools/update-sdk.sh --ios-version 6.3.0 [--export]" >&2
  echo "   or: bash Tools/update-sdk.sh --android-only --android-version 6.3.+" >&2
  exit 1
fi

if [[ "$DO_IOS" -eq 1 && -z "$IOS_VERSION" ]]; then
  echo "ERROR: --ios-version is required unless you pass --android-only" >&2
  exit 1
fi

to_maven_floating() {
  # 6.3.0 -> 6.3.+ ; 6.3 -> 6.3.+ ; 6.3.+ stays
  local v="$1"
  if [[ "$v" == *".+" ]]; then
    echo "$v"
    return
  fi
  if [[ "$v" =~ ^([0-9]+\.[0-9]+)(\.[0-9]+)?([-+].*)?$ ]]; then
    echo "${BASH_REMATCH[1]}.+"
    return
  fi
  echo "${v}.+"
}

current_gradle_version() {
  sed -nE "s/.*['\"]${AAR_COORD}:([^'\"]+)['\"].*/\1/p" "$GRADLE" | head -1
}

set_gradle_version() {
  local new_ver="$1"
  local old
  old="$(current_gradle_version)"
  if [[ -z "$old" ]]; then
    echo "ERROR: cannot find $AAR_COORD in $GRADLE" >&2
    exit 1
  fi
  if [[ "$old" == "$new_ver" ]]; then
    echo "==> Android gradle already at $new_ver"
    return
  fi
  echo "==> Android: $old -> $new_ver"
  if [[ "$DRY_RUN" -eq 1 ]]; then
    return
  fi
  python3 - "$GRADLE" "$AAR_COORD" "$new_ver" <<'PY'
import re, sys
path, coord, ver = sys.argv[1], sys.argv[2], sys.argv[3]
text = open(path).read()
pat = re.compile(re.escape(coord) + r":[^'\"]+")
new, n = pat.subn(f"{coord}:{ver}", text, count=1)
if n != 1:
    raise SystemExit(f"failed to rewrite {coord} in {path}")
open(path, "w").write(new)
PY
}

set_ios_version() {
  local new_ver="$1"
  if [[ ! -f "$POSTPROCESS" ]]; then
    echo "ERROR: missing $POSTPROCESS" >&2
    exit 1
  fi
  echo "==> iOS Swift package: $new_ver"
  if [[ "$DRY_RUN" -eq 1 ]]; then
    return
  fi
  python3 - "$POSTPROCESS" "$new_ver" <<'PY'
import re, sys
path, ver = sys.argv[1], sys.argv[2]
text = open(path).read()
pat = re.compile(r'(private const string IosPackageVersion = ")[^"]+(";)')
new, n = pat.subn(rf'\g<1>{ver}\g<2>', text, count=1)
if n != 1:
    raise SystemExit(f"failed to rewrite IosPackageVersion in {path}")
open(path, "w").write(new)
PY
}

if [[ "$DO_IOS" -eq 1 ]]; then
  set_ios_version "$IOS_VERSION"
fi

if [[ "$DO_ANDROID" -eq 1 ]]; then
  if [[ -n "$ANDROID_VERSION_OVERRIDE" ]]; then
    MAVEN_VER="$ANDROID_VERSION_OVERRIDE"
  elif [[ -n "$IOS_VERSION" ]]; then
    MAVEN_VER="$(to_maven_floating "$IOS_VERSION")"
  else
    echo "ERROR: need --android-version or --ios-version to derive the Maven version" >&2
    exit 1
  fi
  set_gradle_version "$MAVEN_VER"
fi

echo "==> Done."
if [[ "$DO_IOS" -eq 1 ]]; then
  echo "    iOS package: $IOS_VERSION ($POSTPROCESS)"
fi
if [[ "$DO_ANDROID" -eq 1 ]]; then
  if [[ "$DRY_RUN" -eq 1 && -n "${MAVEN_VER:-}" ]]; then
    echo "    Android dependency: $MAVEN_VER (dry-run)"
  else
    echo "    Android dependency: $(current_gradle_version)"
  fi
fi

if [[ "$DO_EXPORT" -eq 1 ]]; then
  EXPORT_VER="$(current_gradle_version)"
  [[ -n "$EXPORT_VER" ]] || EXPORT_VER="${IOS_VERSION}.+"
  echo "==> Exporting unitypackage ($EXPORT_VER)..."
  if [[ "$DRY_RUN" -eq 1 ]]; then
    echo "DRY-RUN: PACKAGE_VERSION=$EXPORT_VER bash Tools/export-unitypackage.sh"
  else
    PACKAGE_VERSION="$EXPORT_VER" bash "$PROJECT_ROOT/Tools/export-unitypackage.sh"
  fi
fi
