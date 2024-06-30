using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Audio :MonoBehaviour
{
    public Sound[] sounds;
    //[SerializeField] Slider slider;
    //public bool isSoundOn = true;
    protected virtual void Start()
    {
        //slider.value = 1;
        foreach (var sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
        }
        //StartIsSoundOn();
    }
    /*
    void StartIsSoundOn()
    {
        if (isSoundOn)
        {
            slider.value = 1;
            foreach (var sound in sounds)
            {
                sound.audioSource.volume = sound.volume;
            }
        }
        else
        {
            slider.value = 0;
            foreach (var sound in sounds)
            {
                sound.audioSource.volume = 0;
            }
        }
    }
    public void OnClickMuteButton()
    {
        SE.Play("pe");
        if (isSoundOn)
        {
            isSoundOn = false;
            slider.value = 0;
            foreach (var sound in sounds)
            {
                sound.audioSource.volume = 0;
            }
        }
        else
        {
            isSoundOn = true;
            slider.value = 1;
            foreach (var sound in sounds)
            {
                sound.audioSource.volume = sound.volume;
            }
        }
        SaveIsSoundOn();
    }
    protected virtual void SaveIsSoundOn()
    {
    }
    */
}


