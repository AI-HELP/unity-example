using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
public class AIHelpBuildPostProcessor
{

	[PostProcessBuildAttribute(1)]
	public static void OnPostProcessBuild(BuildTarget target, string path)
	{
		if (target == BuildTarget.iOS)
		{
			// Read.
			string projectPath = PBXProject.GetPBXProjectPath(path);
			PBXProject project = new PBXProject();
			project.ReadFromString(File.ReadAllText(projectPath));
			string targetName = PBXProject.GetUnityTargetName();
			string projectTarget = project.TargetGuidByName(targetName);

			// Frameworks (webkit and sqlite)
			project.AddFrameworkToProject(projectTarget, "WebKit.framework", true);//将WebKit.framework的引用改成弱引用(当使用webkit的时候才引用)
			project.AddFrameworkToProject(projectTarget, "libsqlite3.tbd", false);

			// Add `-ObjC` to "Other Linker Flags".
			project.AddBuildProperty(projectTarget, "OTHER_LDFLAGS", "-ObjC"); 

            string fullPath = Path.GetFullPath(path);
            //WriteCode(fullPath);
            // ReplaceCode(fullPath);

			// Write.
			File.WriteAllText(projectPath, project.WriteToString());
            
		}
	}

    static void WriteCode(string path)
    {
        string filePath = path + "/Classes/UnityAppController.mm";
        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogError(filePath + "路径下文件不存在");
            return;
        }
        Write("#include \"PluginBase/AppDelegateListener.h\"","#include <ElvaChatServiceSDK/ECServiceSdk.h>",filePath);
        Write("handler(UIBackgroundFetchResultNoData);\n    }","    if ([[[userInfo objectForKey:@\"aps\"] objectForKey:@\"from\"] isEqualToString:@\"elva\"]) {\n        [ECServiceSdk handlePushNotification:userInfo DataFromInApp:YES];\n    }",filePath);
        Write("[[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];","    if (NSDictionary *table = [launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey]) {\n        if ([[[table objectForKey:@\"aps\"] objectForKey:@\"from\"] isEqualToString:@\"elva\"]) {\n            [ECServiceSdk handlePushNotification:table DataFromInApp:NO];\n        }\n    }",filePath);
    }

    static void ReplaceCode(string path)
    {
        string filePath = path + "/Libraries/Plugins/iOS/AIHelpSDK/ElvaUnity.mm";
        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogError(filePath + "路径下文件不存在");
            return;
        }
        //在指定代码中替换一行
        Replace("#import \"ECServiceSdk.h\"", "#import <ElvaChatServiceSDK/ECServiceSdk.h>", filePath);
    }

    static void Write(string below, string text, string filePath)
    {
        StreamReader streamReader = new StreamReader(filePath);
        string text_all = streamReader.ReadToEnd();
        streamReader.Close();

        int beginIndex = text_all.IndexOf(below);
        if (beginIndex == -1)
        {
            Debug.LogError(filePath + "中没有找到标致" + below);
            return;
        }

        int endIndex = text_all.LastIndexOf("\n", beginIndex + below.Length);

        text_all = text_all.Substring(0, endIndex) + "\n" + text + text_all.Substring(endIndex);

        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(text_all);
        streamWriter.Close();
    }

    static void Replace(string below, string newText, string filePath)
    {
        StreamReader streamReader = new StreamReader(filePath);
        string text_all = streamReader.ReadToEnd();
        streamReader.Close();

        int beginIndex = text_all.IndexOf(below);
        if (beginIndex == -1)
        {
            Debug.LogError(filePath + "中没有找到标致" + below);
            return;
        }

        text_all = text_all.Replace(below, newText);
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(text_all);
        streamWriter.Close();
    }
}
#endif
