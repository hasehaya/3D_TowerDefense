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
            if (instance == null)
            {
                instance = new SceneLoader();
            }
            return instance;
        }
    }

    public enum SceneName
    {
        Home = 1,
        StageSelect = 2,
        Stage1 = 11,
        Stage2 = 12,
        Stage3 = 13,
        Stage4 = 14,
        Stage5 = 15,
        Stage6 = 16,
        Stage7 = 17,
        Stage8 = 18,
        Stage9 = 19,
        Stage10 = 20,
        Stage11 = 21,
        Stage12 = 22,
        Stage13 = 23,
    }

    public void RestartStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextStage()
    {
        SceneName nextScene;
        if (SceneManager.GetActiveScene().name == "Stage13")
        {
            nextScene = SceneName.StageSelect;
        }
        else
        {
            nextScene = (SceneName)SceneManager.GetActiveScene().buildIndex + 1;
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
