﻿using System;

using UnityEngine;

public class StageManager :MonoBehaviour
{
    public static Action OnPause;
    public static Action OnResume;

    private static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageManager>();
            }
            return instance;
        }
    }
    [SerializeField] PlayerBase playerBase;
    public int stageNum { get; private set; } = 1;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public Vector3 GetPlayerBasePosition()
    {
        return playerBase.transform.position;
    }

    public void SpeedDown()
    {
        if (Time.timeScale > 0.3f)
        {
            Time.timeScale -= 0.3f;
        }
    }

    public void NomalSpeed()
    {
        Time.timeScale = 1.0f;
    }

    public void SpeedUp()
    {
        Time.timeScale += 0.3f;
    }

    public void StageClear()
    {
        UIManager.Instance.ShowStageClearPanel();
        OnPause?.Invoke();
    }

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverPanel();
        OnPause?.Invoke();
    }

    public void NextStage()
    {
        SceneLoader.Instance.LoadNextStage();
        Time.timeScale = 1.0f;
    }

    public void RestartStage()
    {
        SceneLoader.Instance.RestartStage();
        Time.timeScale = 1.0f;
    }
}
