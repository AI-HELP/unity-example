using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AIHelp.Editor
{
    /// <summary>
    /// 提供菜单: AIHelp -> Build Android APK (IL2CPP / ARM64)
    /// 也提供命令行入口: -executeMethod AIHelp.Editor.AIHelpBuildMenu.BuildAndroidCli
    ///
    /// 适配 Unity 6+: 用 NamedBuildTarget 替代 BuildTargetGroup（老 API 仍然兼容但已标 Obsolete）。
    /// </summary>
    public static class AIHelpBuildMenu
    {
        private const string OutputApk  = "Build/AIHelpDemo.apk";
        private const string DemoScene  = "Assets/Scenes/Demo.unity";

        [MenuItem("AIHelp/Build Android APK (IL2CPP ARM64)")]
        public static void BuildAndroidMenu() => BuildAndroid();

        /// <summary>
        /// 命令行入口: Unity -batchmode -quit -projectPath <path> -executeMethod AIHelp.Editor.AIHelpBuildMenu.BuildAndroidCli
        /// </summary>
        public static void BuildAndroidCli() => BuildAndroid();

        private static void BuildAndroid()
        {
            if (!File.Exists(DemoScene))
            {
                Debug.LogError($"[AIHelpDemo] Scene not found: {DemoScene} (please create it via menu: AIHelp -> Create Demo Scene)");
                EditorApplication.Exit(1);
                return;
            }

            string apkPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), OutputApk));
            Directory.CreateDirectory(Path.GetDirectoryName(apkPath));

            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(DemoScene, true)
            };

            var androidTarget = NamedBuildTarget.Android;
            PlayerSettings.SetScriptingBackend(androidTarget, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.stripEngineCode = false;
            PlayerSettings.SetManagedStrippingLevel(androidTarget, ManagedStrippingLevel.Disabled);

            var options = new BuildPlayerOptions
            {
                scenes = new[] { DemoScene },
                locationPathName = apkPath,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = BuildOptions.None
            };

            Debug.Log($"[AIHelpDemo] Building Android APK -> {apkPath}");
            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                Debug.LogError($"[AIHelpDemo] Build failed: {report.summary.result}, errors={report.summary.totalErrors}");
                EditorApplication.Exit(1);
                return;
            }
            Debug.Log($"[AIHelpDemo] Build OK: {apkPath} ({new FileInfo(apkPath).Length} bytes)");
        }
    }
}
