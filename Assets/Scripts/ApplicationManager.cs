using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ApplicationManager :MonoBehaviour
{
    private static ApplicationManager instance;
    public static ApplicationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ApplicationManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("BGM_Singleton");
                    instance = singletonObject.AddComponent<ApplicationManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, new RefreshRate { numerator = 60, denominator = 1 });
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
