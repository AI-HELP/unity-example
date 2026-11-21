# AIHelp Unity SDK 维护脚本

本目录脚本用于**改 SDK 版本号**和**导出客户用 `.unitypackage`**。iOS 不在仓库里放二进制。

| 脚本 | 作用 | 是否需要开 Unity |
|------|------|------------------|
| `update-sdk.sh` | 改 iOS Swift Package 版本，同步 Android Maven 版本 | 否 |
| `export-unitypackage.sh` | 导出 SDK `.unitypackage` | 是（batchmode，需先退出本工程 Editor） |

相关 Editor 入口（可选）：

- 菜单 `AIHelp → Export SDK Package`
- 实现：`Assets/Editor/AIHelpExportPackage.cs`

---

## iOS 怎么进来

导出 Xcode 工程时，`Assets/Plugins/Editor/Editor/PostProcessBuild.cs` 把这个包加到 UnityFramework：

```
https://github.com/AI-HELP/aihelp-ios-sdk.git
```

当前钉的是 tag `6.3.0`（`upToNextMajor`，仓库里 6.x 只有这一个 tag）。包内产品 `AIHelpSupportSDK` 带静态 xcframework 和 `AIHelpSupportSDK.bundle`。`Assets/Plugins/iOS/AIHelpSDK/` 只留 `AIHelpUnity.h` / `AIHelpUnity.mm`。

不要再把 xcframework 或 bundle 放进 `Plugins/iOS`，否则会和 Swift Package 里的静态库重复链接。

---

## 推荐流程（发新版）

1. **退出**本工程的 Unity Editor
2. 一条命令改版本并导出：

```bash
bash Tools/update-sdk.sh --ios-version 6.3.0 --export
```

完成后：

- iOS 版本写在 `PostProcessBuild.cs` 的 `IosPackageVersion`
- Android 依赖同步为 `net.aihelp:android-aihelp-aar:<major.minor>.+`（如 `6.3.+`）
- 包输出到 `UnityPackages/<版本>.unitypackage`

也可分两步：

```bash
bash Tools/update-sdk.sh --ios-version 6.3.0
bash Tools/export-unitypackage.sh
```

---

## update-sdk.sh

### 基本用法

```bash
bash Tools/update-sdk.sh --ios-version 6.3.0
bash Tools/update-sdk.sh --ios-version 6.3.0 --dry-run
bash Tools/update-sdk.sh --ios-version 6.3.0 --export
bash Tools/update-sdk.sh --android-only --android-version 6.3.+
```

### 会做什么

1. 把 `IosPackageVersion` 改成给定 tag
2. 按这个版本把 `mainTemplate.gradle` 里的 Maven 坐标写成浮动版本，例如 `6.3.0` → `6.3.+`

### 常用参数

```bash
--ios-only                               # 只改 iOS tag
--android-only --android-version 6.3.+   # 只改 Android 依赖
--android-version 6.3.0                  # 覆盖默认的 major.minor.+ 写法
--export
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
PACKAGE_VERSION=6.3.+ \
PACKAGE_OUTPUT=/tmp/AIHelp-6.3.unitypackage \
bash Tools/export-unitypackage.sh
```

### 包内包含 / 不包含

**包含：**

- `Assets/Scripts/AIHelp/`
- `Assets/Plugins/iOS/AIHelpSDK/`（只有 Unity 桥接）
- `Assets/Plugins/Editor/`（含 iOS Swift Package 的 PostProcessBuild）
- Android 三个 gradle 模板

**不包含：** Demo 场景、`AIHelpDemo.cs`、`Assets/Editor/` 构建菜单、`.idea` 等

客户导入后打 iOS 包，Xcode 会按 `IosPackageVersion` 拉取 SDK。

---

## 注意

1. **导出前必须退出**正在打开本工程的 Unity；同一工程不能同时 GUI + batchmode。
2. 若 Unity 不在默认路径，设置 `UNITY_EDITOR` 指向 Hub 里对应版本目录。
3. 换 tag（例如试 `3.4.2-p4`）只改 `IosPackageVersion`。`3.4.2-p4` 的原生接口和当前 `AIHelpUnity.mm` 对不上，iOS 编不过。

---

## 故障排查

| 现象 | 处理 |
|------|------|
| 导出提示工程正被 Editor 占用 | Quit Unity 后再跑 |
| 未找到 Unity | 安装对应版本或设置 `UNITY_EDITOR` |
| 导出拒绝 vendored iOS SDK | 删掉 `Plugins/iOS/AIHelpSDK` 下的 xcframework、bundle、framework |
| Xcode 找不到 `AIHelpSupportSDK.h` | 确认 PostProcessBuild 已把包加到 UnityFramework，并且 Xcode 能访问 GitHub |
| 导出失败 | 查看 `Logs/export-unitypackage.log` |
