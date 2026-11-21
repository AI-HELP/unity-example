# AIHelp Unity SDK 维护脚本

本目录脚本用于**更新原生 SDK** 与**导出客户用 `.unitypackage`**，无需在 Editor 里手动覆盖文件或改版本号。

| 脚本 | 作用 | 是否需要开 Unity |
|------|------|------------------|
| `update-sdk.sh` | 更新 iOS xcframework/bundle，同步 Android Maven 版本 | 否 |
| `export-unitypackage.sh` | 导出 SDK `.unitypackage` | 是（batchmode，需先退出本工程 Editor） |

相关 Editor 入口（可选）：

- 菜单 `AIHelp → Export SDK Package`
- 实现：`Assets/Editor/AIHelpExportPackage.cs`

---

## 推荐流程（发新版）

1. 拿到 iOS 发布包（如 `6.2.1.zip`，内含 `AIHelpSupportSDK.xcframework` + `AIHelpSupportSDK.bundle`）
2. **退出**本工程的 Unity Editor
3. 一条命令更新并导出：

```bash
bash Tools/update-sdk.sh ~/Desktop/6.2.1.zip --export
```

完成后：

- iOS 资源已写入 `Assets/Plugins/iOS/AIHelpSDK/`
- Android 依赖已同步为 `net.aihelp:android-aihelp-aar:<major.minor>.+`（如 `6.2.+`）
- 包输出到 `UnityPackages/<版本>.unitypackage`

也可分两步：

```bash
bash Tools/update-sdk.sh ~/Desktop/6.2.1.zip
bash Tools/export-unitypackage.sh
```

---

## update-sdk.sh

### 基本用法

```bash
# zip 或已解压目录均可
bash Tools/update-sdk.sh ~/Desktop/6.2.1.zip
bash Tools/update-sdk.sh ~/Desktop/AIHelpSupportSDK

# 先看会改什么，不落盘
bash Tools/update-sdk.sh ~/Desktop/6.2.1.zip --dry-run

# 更新后立刻导出 unitypackage
bash Tools/update-sdk.sh ~/Desktop/6.2.1.zip --export
```

### 会做什么

1. 用发布包里的 **xcframework + bundle** 覆盖 `Assets/Plugins/iOS/AIHelpSDK/`
2. **保留**旁边的 `.meta`（PluginImporter / GUID），以及 `AIHelpUnity.h` / `AIHelpUnity.mm`
3. 删除遗留的 `AIHelpSupportSDK.framework`（若存在）和 `.DS_Store`
4. 按 iOS 版本把 `mainTemplate.gradle` 里的 Maven 坐标写成浮动版本，例如 `6.2.1` → `6.2.+`  
   （同 minor 下一般不必再手改 Android 版本号）

### 常用参数

```bash
--ios-only                          # 只更新 iOS
--android-only --android-version 6.3.+   # 只改 Android 依赖
--version 6.2.1                     # 强制指定版本（无法从包内读到时）
--android-version 6.2.1             # 覆盖默认的 major.minor.+ 写法
--export                            # 结束后调用 export-unitypackage.sh
--dry-run
-h / --help
```

---

## export-unitypackage.sh

### 基本用法

```bash
# 先退出本工程 Unity Editor
bash Tools/export-unitypackage.sh
```

默认：

- Unity：`/Applications/Unity/Hub/Editor/6000.5.1f1`（可用环境变量改）
- 版本：读取 `mainTemplate.gradle` 中的 `android-aihelp-aar` 版本
- 输出：`UnityPackages/<版本>.unitypackage`
- 日志：`Logs/export-unitypackage.log`

### 环境变量

```bash
UNITY_EDITOR=/Applications/Unity/Hub/Editor/6000.5.1f1 \
PACKAGE_VERSION=6.2.+ \
PACKAGE_OUTPUT=/tmp/AIHelp-6.2.unitypackage \
bash Tools/export-unitypackage.sh
```

### 包内包含 / 不包含

**包含：**

- `Assets/Scripts/AIHelp/`
- `Assets/Plugins/iOS/AIHelpSDK/`（xcframework + bundle + bridge）
- `Assets/Plugins/Editor/`
- Android 三个 gradle 模板

**不包含：** Demo 场景、`AIHelpDemo.cs`、`Assets/Editor/` 构建菜单、`.idea` 等

包内会带上关键资源的 `.meta`（Unity 存为 GUID 目录下的 `asset.meta`）。xcframework 内部二进制没有单独 meta，客户导入时内部条目可能标 New，属正常现象。

---

## 注意

1. **导出前必须退出**正在打开本工程的 Unity；同一工程不能同时 GUI + batchmode。
2. 更新 iOS 时**不要删** `*.xcframework.meta` / `*.bundle.meta`；脚本已自动保留。客户通过 Import Package 接入即可，一般无需自己重建 meta。
3. 当前 iOS xcframework 含 `ios-arm64` + `ios-arm64-simulator`：真机与 M 芯片模拟器可用；**Intel Mac 模拟器**无 x86_64 slice。
4. 若 Unity 不在默认路径，设置 `UNITY_EDITOR` 指向 Hub 里对应版本目录。

---

## 故障排查

| 现象 | 处理 |
|------|------|
| 导出提示工程正被 Editor 占用 | Quit Unity 后再跑 |
| 未找到 Unity | 安装对应版本或设置 `UNITY_EDITOR` |
| update 报缺少 xcframework | 确认 zip/目录结构含 `AIHelpSupportSDK.xcframework` |
| 导出后仍有旧 `.framework` | 用 `update-sdk.sh` 更新一次，或手动删掉旧 framework |
| 导出失败 | 查看 `Logs/export-unitypackage.log` |
