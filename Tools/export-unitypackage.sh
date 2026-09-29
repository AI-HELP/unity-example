#!/bin/bash
# Export AIHelp SDK .unitypackage (no Demo / Scenes)
# Usage:
#   bash Tools/export-unitypackage.sh
#   PACKAGE_VERSION=6.2.+ bash Tools/export-unitypackage.sh
#   PACKAGE_OUTPUT=/tmp/aihelp.unitypackage bash Tools/export-unitypackage.sh
# Quit Unity Editor first (same project cannot be open in GUI + batchmode).
set -euo pipefail

PROJECT_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
UNITY_EDITOR="${UNITY_EDITOR:-/Applications/Unity/Hub/Editor/6000.5.1f1}"
UNITY="$UNITY_EDITOR/Unity.app/Contents/MacOS/Unity"
PACKAGE_VERSION="${PACKAGE_VERSION:-}"
# Prefer version already written into mainTemplate.gradle (kept in sync by update-sdk.sh)
if [[ -z "$PACKAGE_VERSION" ]]; then
  PACKAGE_VERSION="$(sed -nE "s/.*['\"]net\.aihelp:android-aihelp-aar:([^'\"]+)['\"].*/\1/p" \
    "$PROJECT_ROOT/Assets/Plugins/Android/mainTemplate.gradle" | head -1)"
fi
PACKAGE_VERSION="${PACKAGE_VERSION:-6.2.+}"
PACKAGE_OUTPUT="${PACKAGE_OUTPUT:-$PROJECT_ROOT/UnityPackages/${PACKAGE_VERSION}.unitypackage}"
LOG_FILE="$PROJECT_ROOT/Logs/export-unitypackage.log"

mkdir -p "$PROJECT_ROOT/UnityPackages" "$PROJECT_ROOT/Logs"

if [[ ! -x "$UNITY" ]]; then
  echo "未找到 Unity: $UNITY"
  echo "设置 UNITY_EDITOR，例如: UNITY_EDITOR=/Applications/Unity/Hub/Editor/6000.5.1f1"
  exit 1
fi

# Avoid clashing with an open Editor on this project
if pgrep -lf "Unity.app/Contents/MacOS/Unity" 2>/dev/null | grep -q "$PROJECT_ROOT"; then
  echo "请先退出正在打开本工程的 Unity Editor，再运行本脚本。"
  exit 1
fi

echo "==> 导出 SDK package: $PACKAGE_OUTPUT"
echo "    版本: $PACKAGE_VERSION"
echo "    日志: $LOG_FILE"

set +e
"$UNITY" \
  -batchmode -quit -nographics \
  -projectPath "$PROJECT_ROOT" \
  -executeMethod AIHelp.Editor.AIHelpExportPackage.ExportCli \
  -packageVersion "$PACKAGE_VERSION" \
  -packageOutput "$PACKAGE_OUTPUT" \
  -logFile "$LOG_FILE"
UNITY_EXIT=$?
set -e

if [[ $UNITY_EXIT -ne 0 || ! -f "$PACKAGE_OUTPUT" ]]; then
  echo ""
  echo "========== 导出失败 =========="
  echo "Unity 退出码: $UNITY_EXIT"
  grep -iE "error|exception|Export package failed|Missing package" "$LOG_FILE" 2>/dev/null \
    | grep -v "Licensing::" | tail -30
  exit 1
fi

echo "==> 成功: $PACKAGE_OUTPUT"
ls -lh "$PACKAGE_OUTPUT"
