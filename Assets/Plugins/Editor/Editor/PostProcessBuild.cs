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
            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);

            // 获取 Main Target (Unity-iPhone)
            string mainTargetGuid = proj.GetUnityMainTargetGuid();

            // 获取 UnityFramework Target
            string frameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();

            // 给 UnityFramework 添加 -ObjC
            if (!string.IsNullOrEmpty(frameworkTargetGuid))
            {
                proj.AddBuildProperty(frameworkTargetGuid, "OTHER_LDFLAGS", "-ObjC");
            }
            else
            {
                UnityEngine.Debug.LogWarning("UnityFramework target GUID is null or empty");
            }

            // （可选）如果需要给主 Target 也加：
            proj.AddBuildProperty(mainTargetGuid, "OTHER_LDFLAGS", "-ObjC");

            proj.WriteToFile(projPath);
        }
    }
}
