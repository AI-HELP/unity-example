#!/bin/bash
# AIHelp Unity Demo - Android IL2CPP / ARM64 build
# Usage: quit Unity, then: bash Tools/build-android.sh
set -euo pipefail

PROJECT_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
# 默认 Unity 6.5 安装路径；可由环境变量覆盖：
#   UNITY_EDITOR=/Applications/Unity/Hub/Editor/6000.5.1f1 bash Tools/build-android.sh
UNITY_EDITOR="${UNITY_EDITOR:-/Applications/Unity/Hub/Editor/6000.5.1f1}"
UNITY="$UNITY_EDITOR/Unity.app/Contents/MacOS/Unity"
APK_PATH="$PROJECT_ROOT/Build/AIHelpDemo.apk"
LOG_FILE="$PROJECT_ROOT/Logs/build-android.log"

mkdir -p "$PROJECT_ROOT/Build" "$PROJECT_ROOT/Logs"

if pgrep -f "Unity.app.*unity-aihelp-demo-6" >/dev/null 2>&1; then
  echo "请先完全退出 Unity Editor（Quit），再运行本脚本。"
  exit 1
fi

if [[ ! -x "$UNITY" ]]; then
  echo "未找到 Unity: $UNITY"
  echo "请用 Unity Hub 安装 Unity 6.5 或设置 UNITY_EDITOR 环境变量。"
  echo "例如: UNITY_EDITOR=/Applications/Unity/Hub/Editor/6000.5.1f1 bash Tools/build-android.sh"
  exit 1
fi

export DOTNET_EnableWriteXorExecute=0
ulimit -n 8192 2>/dev/null || true

# macOS quarantine may kill IL2CPP/Linker sub-processes (exit 137)
xattr -dr com.apple.quarantine "$UNITY_EDITOR/Unity.app/Contents/il2cpp" 2>/dev/null || true
xattr -dr com.apple.quarantine "$UNITY_EDITOR/Unity.app/Contents/Tools" 2>/dev/null || true

mkdir -p "$PROJECT_ROOT/Library/Bee/artifacts/Android/il2cppOutput/cpp/Symbols"

if command -v adb >/dev/null 2>&1; then
  killall adb 2>/dev/null || true
  sleep 1
  adb start-server 2>/dev/null || true
fi

echo "==> 打包中（IL2CPP / ARM64），日志: $LOG_FILE"
set +e
"$UNITY" \
  -batchmode -quit -nographics \
  -projectPath "$PROJECT_ROOT" \
  -executeMethod AIHelp.Editor.AIHelpBuildMenu.BuildAndroidCli \
  -logFile "$LOG_FILE"
UNITY_EXIT=$?
set -e

if [[ ! -f "$APK_PATH" ]]; then
  echo ""
  echo "========== 打包失败 =========="
  echo "Unity 退出码: $UNITY_EXIT"
  echo "常见原因:"
  echo "  - ExitCode 137: IL2CPP 被系统终止 -> 关闭其它 Unity/Chrome, 重新跑 Tools/fix-unitylinker.sh"
  echo "  - Gradle: 检查网络与 mavenCentral (net.aihelp:android-aihelp-aar:5.7.+)"
  echo "  - Unity 版本不匹配: 确认 UNITY_EDITOR 指向 6000.x 实际安装目录"
  echo ""
  grep -iE "error|137|Failed|Gradle|Exception|architecture" "$LOG_FILE" 2>/dev/null | grep -v "Licensing::" | tail -25
  exit 1
fi

echo "==> 成功: $APK_PATH"
ls -lh "$APK_PATH"

if command -v adb >/dev/null 2>&1 && adb devices 2>/dev/null | grep -qE '\tdevice$'; then
  echo "==> 安装到手机..."
  adb install -r "$APK_PATH" && echo "安装完成, 在手机上打开 AIHelpDemo。"
else
  echo "未检测到设备, 可手动: adb install -r \"$APK_PATH\""
fi
