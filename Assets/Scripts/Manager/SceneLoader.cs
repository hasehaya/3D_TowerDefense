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
        Stage1 = 1,
        Stage2 = 2,
        Stage3 = 3,
        Stage4 = 4,
        Stage5 = 5,
        Stage6 = 6,
        Stage7 = 7,
        Stage8 = 8,
        Stage9 = 9,
        Stage10 = 10,
        Stage11 = 11,
        Stage12 = 12,
        Stage13 = 13,


        Home = 101,
        StageSelect = 102,
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
        SharedSceneData.StageNum = (int)nextScene;
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
