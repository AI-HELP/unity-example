using System.IO;
using UnityEditor.Android;
using UnityEngine;

// This processor is only used to enable `android.useAndroidX` and `android.enableJetifier` in `gradle.properties` file.
// If you've configure that via other ways, you can just leave this alone.
public class AIHelpPostBuildProcessor : IPostGenerateGradleAndroidProject
{
    public int callbackOrder
    {
        get { return 999; }
    }
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        string gradlePropertiesFile = path + "/gradle.properties";
        if (File.Exists(gradlePropertiesFile))
        {
            File.Delete(gradlePropertiesFile);
        }
        StreamWriter writer = File.CreateText(gradlePropertiesFile);
        writer.WriteLine("org.gradle.jvmargs=-Xmx4096M");
        writer.WriteLine("android.useAndroidX=true");
        writer.WriteLine("android.enableJetifier=true");
        writer.Flush();
        writer.Close();
    }
}