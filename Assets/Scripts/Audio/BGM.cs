﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BGM :Audio
{
    private static BGM instance;
    public static BGM Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BGM>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("BGM_Singleton");
                    instance = singletonObject.AddComponent<BGM>();
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
        //isSoundOn = DataManager.Instance.Flag.isBgmOn;
        base.Start();
        foreach (var sound in sounds)
        {
            sound.audioSource.loop = true;
        }
        Play("stage1");
    }
    public static void Play(string name)
    {
        Sound sound = Array.Find(BGM.Instance.sounds, sound => sound.name == name);
        sound.audioSource.Play();
    }
    /*
    protected override void SaveIsSoundOn()
    {
        DataManager.Instance.Flag.isBgmOn = isSoundOn;
        DataManager.Instance.Save();
    }
    */
}
