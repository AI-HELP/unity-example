using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

/// <summary>
/// After iOS export:
/// 1) Add -ObjC for AIHelp ObjC categories
/// 2) Force Device+Simulator destinations (Unity Simulator SDK export locks to
///    SDKROOT=iphonesimulator / ARCHS=x86_64, which breaks real devices and M-chip sims)
/// </summary>
public class PostProcessBuild
{
    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.iOS)
        {
            return;
        }

        // Prefer Device SDK for future exports (988 = Device, 989 = Simulator)
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;

        string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string mainTargetGuid = proj.GetUnityMainTargetGuid();
        string frameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();

        foreach (string guid in new[] { mainTargetGuid, frameworkTargetGuid }.Where(g => !string.IsNullOrEmpty(g)))
        {
            proj.AddBuildProperty(guid, "OTHER_LDFLAGS", "-ObjC");
            FixPlatformAndArch(proj, guid);
        }

        proj.WriteToFile(projPath);
        // Must run after WriteToFile — Unity may still stamp Simulator-only into project-level configs
        FixAllProjectBuildConfigs(pathToBuiltProject);

        Debug.Log("[AIHelp] PostProcessBuild: Device+Simulator platforms restored, -ObjC added.");
    }

    private static void FixPlatformAndArch(PBXProject proj, string targetGuid)
    {
        proj.SetBuildProperty(targetGuid, "SDKROOT", "iphoneos");
        proj.SetBuildProperty(targetGuid, "SUPPORTED_PLATFORMS", "iphoneos iphonesimulator");
        proj.SetBuildProperty(targetGuid, "SUPPORTS_MACCATALYST", "NO");
        // Let Xcode pick arm64 (device / M-sim) or x86_64 (Intel sim) — do not hardcode
        proj.SetBuildProperty(targetGuid, "ARCHS", "$(ARCHS_STANDARD)");
        proj.SetBuildProperty(targetGuid, "ONLY_ACTIVE_ARCH", "NO");
        // Clear any leftover excludes
        proj.SetBuildProperty(targetGuid, "EXCLUDED_ARCHS[sdk=iphoneos*]", "");
        proj.SetBuildProperty(targetGuid, "EXCLUDED_ARCHS[sdk=iphonesimulator*]", "");
    }

    private static void FixAllProjectBuildConfigs(string pathToBuiltProject)
    {
        string pbxPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        string text = System.IO.File.ReadAllText(pbxPath);

        text = text.Replace("SDKROOT = iphonesimulator;", "SDKROOT = iphoneos;");
        text = text.Replace(
            "SUPPORTED_PLATFORMS = iphonesimulator;",
            "SUPPORTED_PLATFORMS = \"iphoneos iphonesimulator\";");
        text = text.Replace("ARCHS = x86_64;", "ARCHS = \"$(ARCHS_STANDARD)\";");
        text = text.Replace("ARCHS = arm64;", "ARCHS = \"$(ARCHS_STANDARD)\";");

        System.IO.File.WriteAllText(pbxPath, text);
    }
}
