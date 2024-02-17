#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;


[InitializeOnLoad]
public class AutoSave
{
    static AutoSave()
    {
        EditorApplication.playModeStateChanged += SaveScene;
    }

    private static void SaveScene(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.SaveOpenScenes();
            Debug.Log("saved (⁎ᵕᴗᵕ⁎)");
        }
    }
}
#endif