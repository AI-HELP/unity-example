# AIHelp Unity Demo (Unity 6.5)

最小可运行的 Unity 6.5 demo，把 AIHelp SDK（iOS + Android）接入进来。
代码来自 `../unity-aihelp-demo`，结构完全一致；区别只在编辑器版本和 Build API。

## 跟 unity-aihelp-demo 的差异

| 项 | 原 demo (2021.3) | 本工程 (Unity 6.5) |
| --- | --- | --- |
| `ProjectSettings/ProjectVersion.txt` | `2021.3.45f2` | `6000.5.1f1` (Unity 6.5) |
| `Packages/manifest.json` `com.unity.ugui` | `1.0.0` | `2.0.0` |
| `AIHelpBuildMenu.cs` | 用 `BuildTargetGroup.Android` | 用 `NamedBuildTarget.Android` (Unity 6 推荐的 API) |
| `Tools/build-android.sh` | `UNITY_EDITOR=.../2021.3.45f2` | `UNITY_EDITOR=.../6000.5.1f1` |
| `Tools/fix-unitylinker.sh` | 同上 | 同上 |
| `baseProjectTemplate.gradle` AGP | `4.0.1` | `8.7.3` |
| `gradle/wrapper/gradle-wrapper.properties` | — | `gradle-8.9-bin.zip` (AGP 8.7.x 官方要求 Gradle ≥ 8.9) |
| 源码 / Plugins / Scenes | 完全相同 | 完全相同 |

## 目录结构

```
unity-aihelp-demo-6/
├── Assets/
│   ├── Editor/
│   │   ├── AIHelpBuildMenu.cs        # 菜单: AIHelp -> Build Android APK
│   │   └── AIHelpSceneBuilder.cs     # 菜单: AIHelp -> Create Demo Scene
│   ├── Plugins/
│   │   ├── Android/                  # gradle 模板, 拉 net.aihelp:android-aihelp-aar:5.7.+
│   │   ├── Editor/                   # iOS post-build (-ObjC) + Android post-build
│   │   └── iOS/AIHelpSDK/            # 85M framework + .bundle + .mm bridge
│   ├── Scenes/Demo.unity
│   └── Scripts/
│       ├── AIHelp/                   # SDK C# 封装 (Core/, Config/, AIHelpSupport.cs)
│       ├── AIHelpDemo.cs             # IMGUI 演示入口
│       └── UnityMainThreadDispatcher.cs
├── Build/                            # APK 产物输出
├── Packages/manifest.json
├── ProjectSettings/                  # 27 个 yaml
├── gradle/wrapper/gradle-wrapper.properties  # Gradle 8.9 distributionUrl
├── Tools/
│   ├── build-android.sh              # 一键 Android APK
│   └── fix-unitylinker.sh            # 修 macOS 上 IL2CPP 被 kill 的问题
├── .gitignore
└── README.md
```

## 怎么跑起来

### 1. 用 Unity 6.5 打开
1. Unity Hub → Open → 选 `unity-aihelp-demo-6/`
2. 第一次打开会报 "Opening this project in a newer version of Unity..."，按提示继续
3. 等编译完成（首次会拉 `com.unity.ugui 2.0.0` 等包）

### 2. (可选) 重新生成 Demo 场景
菜单 → **AIHelp → Create Demo Scene**

### 3. 配 AppId
打开 `Assets/Scripts/AIHelpDemo.cs`，把 `androidAppId` / `iosAppId` 改成你 AIHelp 后台申请到的真实值。

### 4. Editor 里跑
打开 Demo 场景直接点 ▶︎。IMGUI 在 Editor 里会真去调底层 API，Android/iOS 路径在 Editor 下被 ifdef 跳过，所以只走 stub 分支，AppId 不匹配不会崩。

### 5. Android 设备跑
```bash
# 先完全退出 Unity, 再执行:
UNITY_EDITOR=/Applications/Unity/Hub/Editor/6000.5.1f1 bash Tools/build-android.sh
# 或在 Editor 菜单: AIHelp -> Build Android APK (IL2CPP ARM64)
```
产物在 `Build/AIHelpDemo.apk`，自动 install 到第一台连着的设备。

### 6. iOS 设备跑
Unity → File → Build Settings → iOS → Build → 选输出目录 → 用 Xcode 打开 → Run。
Build 时会经过 `PostProcessBuild.cs` 自动给 UnityFramework target 加 `-ObjC` linker flag。

## Unity 6.5 已知差异 / 注意事项

- **ProjectVersion.txt** = `6000.5.1f1`（Unity 6.5）。
- **`com.unity.ugui` 升到 2.0.0**：只影响 UGUI，IMGUI 不受影响；本 demo 完全用 IMGUI。
- **`BuildTargetGroup.Android` 标 Obsolete**：已用 `NamedBuildTarget.Android` 替代。
- **AGP / Gradle**：项目锁死 AGP `8.7.3` + Gradle `8.9-bin`。`gradle/wrapper/gradle-wrapper.properties` 是项目根目录里持久化的版本，Unity 6.5 build 时会拷贝到 `Library/Bee/.../gradle/wrapper/gradle-wrapper.properties` 使用。如果发现 build 用的还是老的 9.1.0，说明 Unity 没读到根目录那份，到 Editor → Preferences → External Tools → Android → Gradle 手动指到 `gradle-8.9-bin.zip` 一次。
- **Asset Import Pipeline v2**：Unity 6.5 默认开启，首次导入会慢一些。
- **iOS framework 兼容性**：原 `AIHelpSupportSDK.framework` 是按 2021.3 编的，理论上向后兼容。如果在 Unity 6.5 build iOS 时报链接错误，可能需要从 AIHelp 官网下载最新版 framework 替换。
- **Android AAR 版本**：`net.aihelp:android-aihelp-aar:5.7.+` 在 Unity 6.5 环境下大概率能继续用；如果 Gradle 解析失败，固定成具体版本如 `5.7.10`。

## 常见问题

- **首次打开 Safe Mode**：完全退出 Unity，删 `Library/Bee`，再重开。
- **Android `Could not resolve net.aihelp:android-aihelp-aar:5.7.+`**：检查网络/代理；或固定 AAR 版本。
- **iOS build 找不到 framework**：确认 `Assets/Plugins/iOS/AIHelpSDK/AIHelpSupportSDK.framework` 在新项目里也在；如报架构不兼容，从 AIHelp 官网下新版。
- **build 时 `ExitCode 137` (macOS)**：先退出 Unity，再跑 `bash Tools/fix-unitylinker.sh` 解除 quarantine、清缓存。
