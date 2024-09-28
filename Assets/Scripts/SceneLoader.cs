using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            return instance;
        }
    }

    public enum SceneName
    {
        Home,
        StageSelect,
        Game,
        Stage1,
        Stage2,
        Stage3,
    }

    public void RestartStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextStage()
    {
        SceneName nextScene = SceneName.Home;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":
            nextScene = SceneName.Stage2;
            break;
            case "Stage2":
            nextScene = SceneName.Stage3;
            break;
            case "Stage3":
            nextScene = SceneName.Home;
            break;
        }
        SceneManager.LoadScene(nextScene.ToString());
    }

    public IEnumerator LoadScene(SceneName sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName.ToString());

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        switch (sceneName)
        {
            case SceneName.StageSelect:
            HomeCamera.Instance.SetToSelect();
            break;
        }
    }
}
