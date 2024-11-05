using System;
using System.Linq;

using UnityEngine;

public class WaveManager
{
    private static WaveManager instance;
    public static WaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WaveManager();
            }
            return instance;
        }
    }

    public static Action<int, int> OnWaveChanged;

    WaveDataListEntity waveDataListEntity;
    WaveData[] waveDataList;
    WaveEnemyDataListEntity waveEnemyDataListEntity;
    WaveEnemyData[] waveEnemyList;

    // 生成用
    float spwanTime = 0f;
    int enemyIndex = 0;
    int maxEnemyIndex { get { return waveEnemyList.Length; } }

    int waveIndex = 1;
    int maxWaveIndex { get { return waveDataList.Length; } }
    bool isWaveEnd = false;

    bool isPaused = false;


    public WaveManager()
    {
        waveDataListEntity = ScriptableObjectManager.Instance.GetWaveDataListEntity();
        waveEnemyDataListEntity = ScriptableObjectManager.Instance.GetWaveEnemyDataListEntity();

        var stage = StageManager.Instance.stageNum;
        waveDataList = waveDataListEntity.lists.Where(waveData => waveData.stage == stage).ToArray();
        ReloadWaveEnemyList();
        OnWaveChanged?.Invoke(waveIndex, maxWaveIndex);

        UpdateCaller.AddUpdateCallback(Update);

        StageManager.OnPause += Pause;
        StageManager.OnResume += Resume;
    }

    ~WaveManager()
    {
        UpdateCaller.RemoveUpdateCallback(Update);

        StageManager.OnPause -= Pause;
        StageManager.OnResume -= Resume;
    }

    private void Update()
    {
        if (isPaused)
        {
            return;
        }

        EnemySpawn();
        WaveEndProcess();
        EnemyEliminatedProcess();
    }

    // 敵を出現させる
    void EnemySpawn()
    {
        spwanTime += Time.deltaTime;
        while (enemyIndex < waveEnemyList.Length && waveEnemyList[enemyIndex].spawnTime <= spwanTime)
        {
            var waveData = waveEnemyList[enemyIndex];
            EnemyManager.Instance.SpawnEnemy(waveData.enemyType, waveData.enemyBaseIndex);
            enemyIndex++;
        }
    }

    /// <summary>
    /// すべての敵を出現させたときの処理
    /// </summary>
    void WaveEndProcess()
    {
        if (isWaveEnd)
        {
            return;
        }
        if (enemyIndex < maxEnemyIndex)
        {
            return;
        }

        isWaveEnd = true;
        // 道中
        if (waveIndex < maxWaveIndex)
        {
            NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.NextWave);
        }
    }

    /// <summary>
    /// Waveの敵を全滅させたときの処理
    /// </summary>
    void EnemyEliminatedProcess()
    {
        if (!isWaveEnd)
        {
            return;
        }
        if (EnemyManager.Instance.GetEnemyCount() != 0)
        {
            return;
        }

        // 道中
        if (waveIndex < maxWaveIndex)
        {
            MoneyManager.Instance.AddMoney(waveDataList[waveIndex].money);
            NextWave();
        }
        // 最終Wave
        else
        {
            StageManager.Instance.StageClear();
        }
    }

    void ReloadWaveEnemyList()
    {
        waveEnemyList = waveEnemyDataListEntity.lists.Where(waveEnemyData => waveEnemyData.stage == StageManager.Instance.stageNum && waveEnemyData.wave == waveIndex).ToArray();
    }

    public void NextWave()
    {
        isWaveEnd = false;
        waveIndex++;
        spwanTime = 0f;
        enemyIndex = 0;
        ReloadWaveEnemyList();
        OnWaveChanged?.Invoke(waveIndex, maxWaveIndex);
        NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.NextWave);
    }

    void Pause()
    {
        isPaused = true;
    }

    void Resume()
    {
        isPaused = false;
    }
}

[System.Serializable]
public class WaveData
{
    public int stage;
    public int wave;
    public int money;

    public WaveData()
    {
        stage = 0;
        wave = 0;
        money = 0;
    }
}

[System.Serializable]
public class WaveEnemyData
{
    public int stage;
    public int wave;
    public float spawnTime;
    public EnemyType enemyType;
    public int enemyBaseIndex;

    public WaveEnemyData()
    {
        stage = 0;
        wave = 0;
        spawnTime = 0f;
        enemyType = EnemyType.None;
        enemyBaseIndex = 0;
    }
}
