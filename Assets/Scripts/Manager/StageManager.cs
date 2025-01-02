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

    const float CHANGE_TIME_SCALE = 0.5f;

    [SerializeField] PlayerBase playerBase;

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

    private void OnDestroy()
    {
        // イベントハンドラを解除
        OnPause = null;
        OnResume = null;

        // シングルトンインスタンスを削除
        if (instance == this)
        {
            instance = null;
        }

        // 各マネージャーのインスタンスを明示的に削除
        WaveManager.DestroyInstance();
        EnemyManager.DestroyInstance();
        FacilityManager.DestroyInstance();
        TutorialManager.DestroyInstance();
    }

    void SingletonInitialize()
    {
        var waveManager = WaveManager.Instance;
        var enemyManager = EnemyManager.Instance;
        var facilityManager = FacilityManager.Instance;
        var tutorialManager = TutorialManager.Instance;
    }

    public Vector3 GetPlayerBasePosition()
    {
        return playerBase.transform.position;
    }

    public void SpeedDown()
    {
        if (Time.timeScale > CHANGE_TIME_SCALE)
        {
            Time.timeScale -= CHANGE_TIME_SCALE;
        }
    }

    public void NomalSpeed()
    {
        Time.timeScale = 1.0f;
    }

    public void SpeedUp()
    {
        Time.timeScale += CHANGE_TIME_SCALE;
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

        var clearStageNum = SaveDataManager.Instance.SaveData.ClearStageNum;
        var currentStageNum = SharedSceneData.StageNum;
        if (clearStageNum < currentStageNum)
        {
            SaveDataManager.Instance.SaveData.ClearStageNum = currentStageNum;
            SaveDataManager.Instance.Save();
        }
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
