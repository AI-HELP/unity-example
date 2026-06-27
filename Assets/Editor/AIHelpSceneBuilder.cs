using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AIHelp.Editor
{
    /// <summary>
    /// 第一次打开项目时, 用菜单 AIHelp -> Create Demo Scene 一键生成可运行的 Demo.unity 场景。
    /// 场景只放一个 Camera + 一个挂着 AIHelpDemo 的 GameObject, IMGUI 自动出按钮, 避免手写 .unity YAML。
    /// </summary>
    public static class AIHelpSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/Demo.unity";

        [MenuItem("AIHelp/Create Demo Scene")]
        public static void CreateDemoScene()
        {
            Directory.CreateDirectory("Assets/Scenes");

            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var go = new GameObject("AIHelpDemo");
            go.AddComponent<AIHelpDemo>();
            // 让 Main Camera 渲染纯色背景, IMGUI 不需要灯光
            var cam = Camera.main;
            if (cam != null) cam.clearFlags = CameraClearFlags.SolidColor;

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[AIHelpDemo] Created demo scene at {ScenePath}");
        }
    }
}
