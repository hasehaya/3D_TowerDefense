using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader :MonoBehaviour
{
    private static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneLoader>();
            }
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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
