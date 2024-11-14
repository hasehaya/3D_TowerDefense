using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SE :Audio
{
    private static SE instance;

    public static SE Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SE>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("SE_Singleton");
                    instance = singletonObject.AddComponent<SE>();
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
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    protected override void Start()
    {
        //isSoundOn = DataManager.Instance.Flag.isSeOn;
        base.Start();
    }
    public static void Play(string seName)
    {
        Sound sound = Array.Find(SE.Instance.sounds, sound => sound.name == seName);
        sound.audioSource.Play();
    }

    /*
    protected override void SaveIsSoundOn()
    {
        DataManager.Instance.Flag.isSeOn = isSoundOn;
        DataManager.Instance.Save();
    }
    */
}