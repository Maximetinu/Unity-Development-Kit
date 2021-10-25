using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace UDK.Editor
{
    public static class ProjectTools
    {
        const string menu_preffix = "UDK/Project Tools/";

        [MenuItem(menu_preffix + "Force save project scenes", false, 11)]
        static void ForceSaveAllProjectScenes()
        {
            ForEachScene((Scene scene) =>
            {
                try
                {
                    EditorSceneManager.SaveScene(scene);
                    Debug.Log("Saved scene of name " + scene.name);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message);
                }
            });
        }

        static void ForEachScene(System.Action<Scene> callback)
        {
            string[] scenesGUIDs = AssetDatabase.FindAssets("t:Scene");

            foreach (string guid in scenesGUIDs)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guid);
                EditorSceneManager.OpenScene(scenePath);
                callback(EditorSceneManager.GetActiveScene());
            }
        }
    }
}