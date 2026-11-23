# AIHelp Unity SDK 维护脚本

本目录脚本用于**改 SDK 版本号**和**导出客户用 `.unitypackage`**。iOS 不在仓库里放二进制。

> 本分支：Unity 6 工程壳 + **3.x / 3.4.2** 桥接与 C# API。iOS SPM tag 默认占位 `0.0.0`（接入时改成真实 tag；桥接对应原生 3.4.2）；Android Maven `3.4.+`。

| 脚本 | 作用 | 是否需要开 Unity |
|------|------|------------------|
| `update-sdk.sh` | 改 iOS Swift Package 版本，同步 Android Maven 版本 | 否 |
| `export-unitypackage.sh` | 导出 SDK `.unitypackage` | 是（batchmode，需先退出本工程 Editor） |

相关 Editor 入口（可选）：

- 菜单 `AIHelp → Export SDK Package`
- 实现：`Assets/Editor/AIHelpExportPackage.cs`

---

## iOS 怎么进来

导出 Xcode 工程时，`Assets/Plugins/Editor/Editor/PostProcessBuild.cs` 通过 SPM 拉取：

```
https://github.com/AI-HELP/aihelp-ios-sdk.git
```

当前钉的是占位 tag `0.0.0`（请改成真实发布 tag）：

- `AIHelpSupportSDKCore` → UnityFramework（SDK 二进制）
- `AIHelpResources` → App target（资源 bundle 进 `.app`）

`Assets/Plugins/iOS/AIHelpSDK/` 只留 `AIHelpUnity.h` / `AIHelpUnity.mm`（3.x 桥接）。

不要再把 xcframework 或 bundle 放进 `Plugins/iOS`，否则会和 Swift Package 里的静态库重复链接。

---

## 推荐流程（发新版）

1. **退出**本工程的 Unity Editor
2. 改版本并导出（**注意**：本分支 iOS tag 与 Android 主版本不一致，须显式指定 Android）：

```bash
bash Tools/update-sdk.sh --ios-version 0.0.0 --android-version 3.4.+ --export
```

完成后：

- iOS 版本写在 `PostProcessBuild.cs` 的 `IosPackageVersion`
- Android 依赖为 `net.aihelp:android-aihelp-aar:3.4.+`
- 包输出到 `UnityPackages/<版本>.unitypackage`

也可分两步：

```bash
bash Tools/update-sdk.sh --ios-version 0.0.0 --android-version 3.4.+
bash Tools/export-unitypackage.sh
```

---

## update-sdk.sh

### 基本用法

```bash
bash Tools/update-sdk.sh --ios-version 0.0.0 --android-version 3.4.+
bash Tools/update-sdk.sh --ios-version 0.0.0 --android-version 3.4.+ --dry-run
bash Tools/update-sdk.sh --ios-version 0.0.0 --android-version 3.4.+ --export
bash Tools/update-sdk.sh --android-only --android-version 3.4.+
```

### 会做什么

1. 把 `IosPackageVersion` 改成给定 tag
2. 默认按 iOS 版本推导 Android `major.minor.+`；本分支 SPM `0.0.0` 会错成 `0.0.+`，所以**务必**传 `--android-version 3.4.+`

### 常用参数

```bash
--ios-only                                    # 只改 iOS tag
--android-only --android-version 3.4.+        # 只改 Android 依赖
--android-version 3.4.+                       # 覆盖默认的 major.minor.+ 写法
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
PACKAGE_VERSION=3.4.+ \
PACKAGE_OUTPUT=/tmp/AIHelp-3.4.unitypackage \
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
3. 本分支桥接对应原生 **3.4.2**。仓库默认 SPM `0.0.0` 仅为占位，接入前改成真实 tag。不要换成 6.x 的 tag，C# / `AIHelpUnity.mm` 对不上。

---

## 故障排查

| 现象 | 处理 |
|------|------|
| 导出提示工程正被 Editor 占用 | Quit Unity 后再跑 |
| 未找到 Unity | 安装对应版本或设置 `UNITY_EDITOR` |
| 导出拒绝 vendored iOS SDK | 删掉 `Plugins/iOS/AIHelpSDK` 下的 xcframework、bundle、framework |
| Xcode 找不到 `AIHelpSupportSDK.h` | 确认 PostProcessBuild 已把 Core 加到 UnityFramework，并且 Xcode 能访问 GitHub |
| App 里缺 `AIHelpSupportSDK.bundle` | 确认 App target 已链接 `AIHelpResources` |
| 导出失败 | 查看 `Logs/export-unitypackage.log` |
