using System;
using System.Collections.Generic;

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
    public int StageNum { get; private set; } = 1;


    private void Awake()
    {
        SingletonInitialize();
    }

    private void Start()
    {
#if UNITY_EDITOR == false
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }

    void SingletonInitialize()
    {
        var waveManager = WaveManager.Instance;
        var enemyManager = EnemyManager.Instance;
        var facilityManager = FacilityManager.Instance;
        facilityManager.SetAvailableFacilityTypes(new List<Facility.Type> { Facility.Type.Canon, Facility.Type.Magic });
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

    public void Pause()
    {
        OnPause?.Invoke();
    }

    public void Resume()
    {
        OnResume?.Invoke();
    }

    public void StageClear()
    {
        UIManager.Instance.ShowStageClearPanel();
        Pause();
    }

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverPanel();
        Pause();
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
