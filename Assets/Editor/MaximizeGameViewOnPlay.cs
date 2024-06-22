using UnityEditor;
using UnityEditor.Callbacks;

using UnityEngine;

[InitializeOnLoad]
public class MaximizeGameViewOnPlay
{
    private static bool wasMaximized;

    static MaximizeGameViewOnPlay()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            wasMaximized = IsGameViewMaximized();
            SetGameViewMaximized(true);
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            SetGameViewMaximized(wasMaximized);
        }
    }

    private static bool IsGameViewMaximized()
    {
        System.Type gameViewType = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
        EditorWindow gameView = EditorWindow.GetWindow(gameViewType);
        return gameView != null && gameView.maximized;
    }

    private static void SetGameViewMaximized(bool maximized)
    {
        System.Type gameViewType = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
        EditorWindow gameView = EditorWindow.GetWindow(gameViewType);
        if (gameView != null)
        {
            gameView.maximized = maximized;
        }
    }
}
