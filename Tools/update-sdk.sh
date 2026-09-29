#!/bin/bash
# Update AIHelp native SDK in this Unity project (no Editor required).
#
# Typical flow after downloading an iOS release zip (e.g. Desktop/6.2.1.zip):
#   bash Tools/update-sdk.sh ~/Desktop/6.2.1.zip
#   bash Tools/update-sdk.sh ~/Desktop/6.2.1.zip --export
#
# What it does:
#   1) Replace AIHelpSupportSDK.xcframework + .bundle (keep PluginImporter .meta)
#   2) Remove legacy .framework if present
#   3) Strip .DS_Store / __MACOSX
#   4) Sync Android Maven line in mainTemplate.gradle to <major.minor>.+
#      (Maven floating range — no need to bump patch by hand each time)
#
# Options:
#   --ios-only / --android-only
#   --android-version 6.2.+     # override Maven version (default: from iOS / zip name)
#   --version 6.2.1             # force version used for Android sync + package name
#   --export                    # run Tools/export-unitypackage.sh after update
#   --dry-run
set -euo pipefail

PROJECT_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
IOS_DEST="$PROJECT_ROOT/Assets/Plugins/iOS/AIHelpSDK"
GRADLE="$PROJECT_ROOT/Assets/Plugins/Android/mainTemplate.gradle"
AAR_COORD="net.aihelp:android-aihelp-aar"

SRC=""
DO_IOS=1
DO_ANDROID=1
DO_EXPORT=0
DRY_RUN=0
FORCE_VERSION=""
ANDROID_VERSION_OVERRIDE=""

usage() {
  sed -n '2,25p' "$0" | sed 's/^# \?//'
  exit "${1:-0}"
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    -h|--help) usage 0 ;;
    --ios-only) DO_ANDROID=0; shift ;;
    --android-only) DO_IOS=0; shift ;;
    --export) DO_EXPORT=1; shift ;;
    --dry-run) DRY_RUN=1; shift ;;
    --version) FORCE_VERSION="${2:-}"; shift 2 ;;
    --android-version) ANDROID_VERSION_OVERRIDE="${2:-}"; shift 2 ;;
    --) shift; break ;;
    -*)
      echo "Unknown option: $1" >&2
      usage 1
      ;;
    *)
      if [[ -z "$SRC" ]]; then SRC="$1"; else echo "Unexpected arg: $1" >&2; usage 1; fi
      shift
      ;;
  esac
done

if [[ "$DO_IOS" -eq 1 && -z "$SRC" ]]; then
  echo "Usage: bash Tools/update-sdk.sh <ios-sdk.zip|dir> [--export]" >&2
  echo "   or: bash Tools/update-sdk.sh --android-only --android-version 6.2.+" >&2
  exit 1
fi

run() {
  if [[ "$DRY_RUN" -eq 1 ]]; then
    echo "DRY-RUN: $*"
  else
    "$@"
  fi
}

# --- locate xcframework + bundle inside zip or directory ---
find_sdk_root() {
  local root="$1"
  if [[ -d "$root/AIHelpSupportSDK.xcframework" || -d "$root/AIHelpSupportSDK.bundle" ]]; then
    echo "$root"
    return
  fi
  if [[ -d "$root/AIHelpSupportSDK/AIHelpSupportSDK.xcframework" ]]; then
    echo "$root/AIHelpSupportSDK"
    return
  fi
  # one nested folder
  local child
  child="$(find "$root" -maxdepth 2 -type d -name 'AIHelpSupportSDK.xcframework' 2>/dev/null | head -1)"
  if [[ -n "$child" ]]; then
    dirname "$child"
    return
  fi
  return 1
}

read_ios_version() {
  local sdk_root="$1"
  local plist=""
  for cand in \
    "$sdk_root/AIHelpSupportSDK.xcframework/ios-arm64/AIHelpSupportSDK.framework/Info.plist" \
    "$sdk_root/AIHelpSupportSDK.xcframework/ios-arm64-simulator/AIHelpSupportSDK.framework/Info.plist"
  do
    if [[ -f "$cand" ]]; then plist="$cand"; break; fi
  done
  if [[ -z "$plist" ]]; then
    return 1
  fi
  /usr/libexec/PlistBuddy -c 'Print :CFBundleShortVersionString' "$plist" 2>/dev/null \
    || plutil -extract CFBundleShortVersionString raw "$plist" 2>/dev/null
}

to_maven_floating() {
  # 6.2.1 -> 6.2.+ ; 6.2 -> 6.2.+ ; 6.2.+ stays
  local v="$1"
  if [[ "$v" == *".+" ]]; then
    echo "$v"
    return
  fi
  if [[ "$v" =~ ^([0-9]+\.[0-9]+)(\.[0-9]+)?$ ]]; then
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
  # portable in-place replace
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

# --- iOS update ---
VERSION=""
WORKDIR=""
cleanup() {
  if [[ -n "${WORKDIR:-}" && -d "$WORKDIR" ]]; then
    rm -rf "$WORKDIR"
  fi
}
trap cleanup EXIT

if [[ "$DO_IOS" -eq 1 ]]; then
  if [[ ! -e "$SRC" ]]; then
    echo "ERROR: source not found: $SRC" >&2
    exit 1
  fi

  SDK_ROOT=""
  if [[ -f "$SRC" && "$SRC" == *.zip ]]; then
    WORKDIR="$(mktemp -d "${TMPDIR:-/tmp}/aihelp-sdk.XXXXXX")"
    echo "==> Unzip: $SRC"
    unzip -q "$SRC" -d "$WORKDIR"
    # drop macOS resource forks
    find "$WORKDIR" -name '__MACOSX' -type d -prune -exec rm -rf {} + 2>/dev/null || true
    find "$WORKDIR" -name '.DS_Store' -delete 2>/dev/null || true
    SDK_ROOT="$(find_sdk_root "$WORKDIR")" || {
      echo "ERROR: zip has no AIHelpSupportSDK.xcframework" >&2
      exit 1
    }
  elif [[ -d "$SRC" ]]; then
    SDK_ROOT="$(find_sdk_root "$SRC")" || {
      echo "ERROR: dir has no AIHelpSupportSDK.xcframework: $SRC" >&2
      exit 1
    }
  else
    echo "ERROR: source must be a .zip or directory: $SRC" >&2
    exit 1
  fi

  if [[ ! -d "$SDK_ROOT/AIHelpSupportSDK.xcframework" ]]; then
    echo "ERROR: missing xcframework under $SDK_ROOT" >&2
    exit 1
  fi
  if [[ ! -d "$SDK_ROOT/AIHelpSupportSDK.bundle" ]]; then
    echo "ERROR: missing bundle under $SDK_ROOT" >&2
    exit 1
  fi

  VERSION="${FORCE_VERSION:-}"
  if [[ -z "$VERSION" ]]; then
    VERSION="$(read_ios_version "$SDK_ROOT" || true)"
  fi
  if [[ -z "$VERSION" && "$SRC" == *.zip ]]; then
    VERSION="$(basename "$SRC" .zip)"
  fi
  if [[ -z "$VERSION" ]]; then
    echo "ERROR: cannot detect SDK version; pass --version 6.2.1" >&2
    exit 1
  fi

  echo "==> iOS SDK version: $VERSION"
  echo "==> Replace xcframework + bundle in:"
  echo "    $IOS_DEST"
  echo "    (keeping *.meta / AIHelpUnity.*)"

  if [[ "$DRY_RUN" -eq 0 ]]; then
    mkdir -p "$IOS_DEST"
    rm -rf "$IOS_DEST/AIHelpSupportSDK.xcframework"
    rm -rf "$IOS_DEST/AIHelpSupportSDK.bundle"
    # legacy device-only framework must not ship beside xcframework
    rm -rf "$IOS_DEST/AIHelpSupportSDK.framework"
    rm -f "$IOS_DEST/AIHelpSupportSDK.framework.meta"

    ditto "$SDK_ROOT/AIHelpSupportSDK.xcframework" "$IOS_DEST/AIHelpSupportSDK.xcframework"
    ditto "$SDK_ROOT/AIHelpSupportSDK.bundle" "$IOS_DEST/AIHelpSupportSDK.bundle"
    find "$IOS_DEST" -name '.DS_Store' -delete 2>/dev/null || true
    find "$IOS_DEST" -name '__MACOSX' -type d -prune -exec rm -rf {} + 2>/dev/null || true

    # ensure PluginImporter metas still exist (created once; never overwrite GUID)
    if [[ ! -f "$IOS_DEST/AIHelpSupportSDK.xcframework.meta" ]]; then
      echo "WARN: missing AIHelpSupportSDK.xcframework.meta — open Unity once to regenerate, or restore from git." >&2
    fi
    if [[ ! -f "$IOS_DEST/AIHelpSupportSDK.bundle.meta" ]]; then
      echo "WARN: missing AIHelpSupportSDK.bundle.meta — open Unity once to regenerate, or restore from git." >&2
    fi
  fi
fi

# --- Android version sync ---
if [[ "$DO_ANDROID" -eq 1 ]]; then
  if [[ -n "$ANDROID_VERSION_OVERRIDE" ]]; then
    MAVEN_VER="$ANDROID_VERSION_OVERRIDE"
  else
    if [[ -z "${VERSION:-}" ]]; then
      VERSION="${FORCE_VERSION:-}"
    fi
    if [[ -z "${VERSION:-}" ]]; then
      echo "ERROR: need --version or iOS source to derive Android version" >&2
      exit 1
    fi
    MAVEN_VER="$(to_maven_floating "$VERSION")"
  fi
  set_gradle_version "$MAVEN_VER"
fi

echo "==> Done."
if [[ "$DO_IOS" -eq 1 ]]; then
  echo "    iOS:  $IOS_DEST (v${VERSION:-?})"
fi
if [[ "$DO_ANDROID" -eq 1 ]]; then
  echo "    Android dependency: $(current_gradle_version)"
fi

if [[ "$DO_EXPORT" -eq 1 ]]; then
  EXPORT_VER="$(current_gradle_version)"
  [[ -n "$EXPORT_VER" ]] || EXPORT_VER="${VERSION}.+"
  echo "==> Exporting unitypackage ($EXPORT_VER)..."
  if [[ "$DRY_RUN" -eq 1 ]]; then
    echo "DRY-RUN: PACKAGE_VERSION=$EXPORT_VER bash Tools/export-unitypackage.sh"
  else
    PACKAGE_VERSION="$EXPORT_VER" bash "$PROJECT_ROOT/Tools/export-unitypackage.sh"
  fi
fi
