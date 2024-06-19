using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class PostProcessBuild
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
            // Path to the Xcode project
            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            
            // Read the Xcode project
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);

            // Get the main target GUID
            string mainTargetName = PBXProject.GetUnityTargetName();
            string mainTargetGuid = proj.TargetGuidByName(mainTargetName);
            if (string.IsNullOrEmpty(mainTargetGuid))
            {
                UnityEngine.Debug.LogError("Main target GUID is null or empty");
                return;
            }

            // Add -ObjC to Other Linker Flags for the main target
            proj.AddBuildProperty(mainTargetGuid, "OTHER_LDFLAGS", "-ObjC");

            // Optional: Get the framework target GUID and add -ObjC to its Other Linker Flags
            string frameworkTargetGuid = proj.TargetGuidByName("UnityFramework");
            if (!string.IsNullOrEmpty(frameworkTargetGuid))
            {
                proj.AddBuildProperty(frameworkTargetGuid, "OTHER_LDFLAGS", "-ObjC");
            }
            else
            {
                UnityEngine.Debug.LogWarning("UnityFramework target GUID is null or empty");
            }

            // Write the Xcode project file back
            proj.WriteToFile(projPath);
        }
    }
}
