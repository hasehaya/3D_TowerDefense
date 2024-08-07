﻿using System.Collections;
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
            }
            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, new RefreshRate { numerator = 60, denominator = 1 });
    }
}
