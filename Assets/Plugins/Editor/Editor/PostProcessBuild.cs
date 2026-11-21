using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

/// <summary>
/// After iOS export:
/// 1) Pull AIHelp with Swift Package Manager: Core on UnityFramework, resources on the App
/// 2) Add -ObjC for AIHelp ObjC categories
/// 3) Force Device+Simulator destinations (Unity Simulator SDK export locks to
///    SDKROOT=iphonesimulator / ARCHS=x86_64, which breaks real devices and M-chip sims)
/// </summary>
public class PostProcessBuild
{
    // Tools/update-sdk.sh rewrites IosPackageVersion. Keep this declaration on one line.
    private const string IosPackageUrl = "https://github.com/AI-HELP/aihelp-ios-sdk.git";
    private const string IosPackageVersion = "0.0.4";
    private const string IosPackageProductCore = "AIHelpSupportSDKCore";
    private const string IosPackageProductResources = "AIHelpResources";

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

        // UnityFramework runs the SDK binary. The App target links only the resource
        // product, which is what makes Xcode copy the bundle into the .app.
        if (string.IsNullOrEmpty(frameworkTargetGuid) || string.IsNullOrEmpty(mainTargetGuid))
        {
            Debug.LogError("[AIHelp] Unity target missing; Swift package was not added.");
        }
        else
        {
            string packageGuid = proj.AddRemotePackageReferenceAtVersion(IosPackageUrl, IosPackageVersion);
            proj.AddRemotePackageFrameworkToProject(frameworkTargetGuid, IosPackageProductCore, packageGuid, false);
            proj.AddRemotePackageFrameworkToProject(mainTargetGuid, IosPackageProductResources, packageGuid, false);
        }

        proj.WriteToFile(projPath);
        // Must run after WriteToFile — Unity may still stamp Simulator-only into project-level configs
        FixAllProjectBuildConfigs(pathToBuiltProject);

        Debug.Log("[AIHelp] PostProcessBuild: SPM " + IosPackageProductCore + " + " + IosPackageProductResources
            + " " + IosPackageVersion + ", Device+Simulator platforms restored, -ObjC added.");
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
