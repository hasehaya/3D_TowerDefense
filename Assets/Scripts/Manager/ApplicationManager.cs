using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ApplicationManager
{
    private static ApplicationManager instance;
    public static ApplicationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ApplicationManager();
            }
            return instance;
        }
    }

    private ApplicationManager()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, new RefreshRate { numerator = 60, denominator = 1 });
    }
}
