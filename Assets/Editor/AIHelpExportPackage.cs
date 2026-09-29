using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AIHelp.Editor
{
    /// <summary>
    /// 导出 AIHelp SDK .unitypackage（不含 Demo / Scenes）。
    /// 菜单: AIHelp -> Export SDK Package
    /// CLI: -executeMethod AIHelp.Editor.AIHelpExportPackage.ExportCli
    /// 可选参数: -packageVersion 6.2.+  -packageOutput /abs/path/out.unitypackage
    /// </summary>
    public static class AIHelpExportPackage
    {
        private const string DefaultVersion = "6.2.+";
        private const string OutputDir = "UnityPackages";
        private const string GradleTemplate = "Assets/Plugins/Android/mainTemplate.gradle";
        private const string AndroidCoord = "net.aihelp:android-aihelp-aar:";

        private static readonly string[] PackageRoots =
        {
            "Assets/Scripts/AIHelp",
            "Assets/Plugins/iOS/AIHelpSDK",
            "Assets/Plugins/Editor",
            "Assets/Plugins/Android/baseProjectTemplate.gradle",
            "Assets/Plugins/Android/mainTemplate.gradle",
            "Assets/Plugins/Android/gradleTemplate.properties",
        };

        [MenuItem("AIHelp/Export SDK Package")]
        public static void ExportMenu()
        {
            try
            {
                string path = ExportInternal(ResolveDefaultVersion(), null);
                EditorUtility.RevealInFinder(path);
            }
            catch (Exception e)
            {
                Debug.LogError($"[AIHelp] Export package failed: {e.Message}");
                EditorUtility.DisplayDialog("Export SDK Package", e.Message, "OK");
            }
        }

        /// <summary>
        /// Unity -batchmode -quit -projectPath &lt;path&gt; -executeMethod AIHelp.Editor.AIHelpExportPackage.ExportCli
        /// </summary>
        public static void ExportCli()
        {
            try
            {
                string version = ReadArg("-packageVersion") ?? ResolveDefaultVersion();
                string output = ReadArg("-packageOutput");
                string path = ExportInternal(version, output);
                Debug.Log($"[AIHelp] Exported: {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[AIHelp] Export package failed: {e}");
                if (Application.isBatchMode)
                {
                    EditorApplication.Exit(1);
                }
            }
        }

        private static string ExportInternal(string version, string outputOverride)
        {
            string[] missing = PackageRoots.Where(p => !AssetExists(p)).ToArray();
            if (missing.Length > 0)
            {
                throw new FileNotFoundException("Missing package assets:\n" + string.Join("\n", missing));
            }

            // Prefer xcframework; fail loudly if old framework is still present (avoid shipping both)
            if (Directory.Exists("Assets/Plugins/iOS/AIHelpSDK/AIHelpSupportSDK.framework")
                || AssetDatabase.IsValidFolder("Assets/Plugins/iOS/AIHelpSDK/AIHelpSupportSDK.framework"))
            {
                throw new InvalidOperationException(
                    "Found legacy AIHelpSupportSDK.framework — remove it and keep only AIHelpSupportSDK.xcframework before exporting.");
            }

            string safeVersion = string.Join("_", version.Split(Path.GetInvalidFileNameChars()));
            string defaultName = Path.Combine(OutputDir, $"{safeVersion}.unitypackage");
            string outPath = string.IsNullOrWhiteSpace(outputOverride)
                ? Path.GetFullPath(defaultName)
                : Path.GetFullPath(outputOverride);

            Directory.CreateDirectory(Path.GetDirectoryName(outPath) ?? OutputDir);

            // Finder junk must not ship; also causes false "New" rows on import
            RemoveJunkFilesUnder("Assets/Plugins/iOS/AIHelpSDK");

            AssetDatabase.ExportPackage(
                PackageRoots,
                outPath,
                ExportPackageOptions.Recurse);

            if (!File.Exists(outPath))
            {
                throw new IOException($"ExportPackage reported success but file missing: {outPath}");
            }

            Debug.Log($"[AIHelp] SDK package -> {outPath} ({new FileInfo(outPath).Length} bytes)");
            return outPath;
        }

        private static string ResolveDefaultVersion()
        {
            try
            {
                if (File.Exists(GradleTemplate))
                {
                    foreach (string line in File.ReadAllLines(GradleTemplate))
                    {
                        int idx = line.IndexOf(AndroidCoord, StringComparison.Ordinal);
                        if (idx < 0)
                        {
                            continue;
                        }
                        int start = idx + AndroidCoord.Length;
                        int end = start;
                        while (end < line.Length && line[end] != '\'' && line[end] != '"')
                        {
                            end++;
                        }
                        if (end > start)
                        {
                            return line.Substring(start, end - start);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[AIHelp] Failed to read version from gradle: {e.Message}");
            }
            return DefaultVersion;
        }

        private static bool AssetExists(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return true;
            }
            return !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(path));
        }

        private static void RemoveJunkFilesUnder(string root)
        {
            if (!Directory.Exists(root))
            {
                return;
            }

            foreach (string path in Directory.EnumerateFiles(root, ".DS_Store", SearchOption.AllDirectories))
            {
                File.Delete(path);
                Debug.Log($"[AIHelp] Removed junk before export: {path}");
            }
        }

        private static string ReadArg(string name)
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == name)
                {
                    return args[i + 1];
                }
            }
            return null;
        }
    }
}
