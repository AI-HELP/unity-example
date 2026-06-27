#!/bin/bash
# 修 UnityLinker / il2cpp 被 macOS kill 的问题。
# 用法: 先完全退出 Unity, 再执行: bash Tools/fix-unitylinker.sh

set -euo pipefail

PROJECT_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
UNITY_EDITOR="${UNITY_EDITOR:-/Applications/Unity/Hub/Editor/6000.5.1f1}"
UNITY_APP="$UNITY_EDITOR/Unity.app"

if pgrep -f "Unity.app.*unity-aihelp-demo-6" >/dev/null 2>&1; then
  echo "检测到 Unity 正在打开本项目, 请先 Quit Unity 后再运行本脚本。"
  exit 1
fi

echo "==> 1) 解除 macOS quarantine (修 exit 137 / killed by macOS)"
for path in \
  "$UNITY_APP/Contents/il2cpp" \
  "$UNITY_APP/Contents/Tools" \
  "$UNITY_APP/Contents/MonoBleedingEdge" \
  "$UNITY_APP/Contents/Frameworks" \
  "$UNITY_APP/Contents/Resources"; do
  if [[ -e "$path" ]]; then
    xattr -dr com.apple.quarantine "$path" 2>/dev/null || true
  fi
done
echo "    OK"

GEN_DIR="$UNITY_APP/Contents/Tools/BuildPlayerDataGenerator"
GEN_EXE="$GEN_DIR/BuildPlayerDataGenerator.exe"
GEN_REAL="$GEN_DIR/BuildPlayerDataGenerator.exe.real"
echo "==> 2) 恢复 BuildPlayerDataGenerator.exe (勿用 shell 替换 .exe)"
# Unity 通过 netcorerun 把 .exe 当 .NET 程序加载；若曾用 shell 包装会报:
#   BadImageFormatException: Bad IL format ... BuildPlayerDataGenerator.exe
if [[ -f "$GEN_REAL" ]]; then
  rm -f "$GEN_EXE"
  mv "$GEN_REAL" "$GEN_EXE"
  echo "    OK (removed bad shell wrapper, restored PE)"
else
  echo "    OK (no wrapper to remove)"
fi

echo "==> 3) 清掉 Bee / BuildPlayerData 缓存"
rm -rf "$PROJECT_ROOT/Library/Bee"
rm -rf "$PROJECT_ROOT/Library/BuildPlayerData"
rm -rf "$PROJECT_ROOT/Library/PlayerDataCache"
rm -rf "$PROJECT_ROOT/Library/Artifacts"
echo "    OK"

echo ""
echo "==> 下一步: 用 Unity Hub 重新打开 unity-aihelp-demo-6, 再 Build。"
echo "   若仍报 class layout incompatible, 看 Logs/build-android.log 是否含 BadImageFormat"
echo "   (说明 .exe 又被替换成脚本, 请再跑一次本脚本恢复)。"
echo "   若 exit 137, 关掉其它占用内存的应用或重新跑本脚本。"
