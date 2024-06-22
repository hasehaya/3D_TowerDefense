using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class WaveManager :MonoBehaviour
{
    [SerializeField] Transform[] enemyBases;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] WaveDataListEntity waveDataListEntity;
    WaveData[] waveDataList;
    [SerializeField] WaveEnemyDataListEntity waveEnemyDataListEntity;
    WaveEnemyData[] waveEnemyDataList;

    //　生成用
    float spwanTime = 0f;
    int spawnIndex = 0;

    int waveIndex = 1;
    bool isWaveEnd = false;

    private void Start()
    {
        var stage = StageManager.Instance.stageNum;
        waveDataList = waveDataListEntity.lists.Where(waveData => waveData.stage == stage).ToArray();
        waveEnemyDataList = waveEnemyDataListEntity.lists.Where(waveEnemyData => waveEnemyData.stage == stage && waveEnemyData.wave == waveIndex).ToArray();
    }

    private void Update()
    {
        spwanTime += Time.deltaTime;
        while (spawnIndex < waveEnemyDataList.Length && waveEnemyDataList[spawnIndex].spawnTime <= spwanTime)
        {
            var waveData = waveEnemyDataList[spawnIndex];
            EnemyManager.Instance.SpawnEnemy(waveData.enemyType, GetEnemyBase(waveData.enemyBaseIndex).position);
            spawnIndex++;
        }
    }

    public Transform GetEnemyBase(int index)
    {
        return enemyBases[index];
    }

    void NextWave()
    {
        waveIndex++;
        if (waveIndex >= waveDataList.Length)
        {
            isWaveEnd = true;
            return;
        }
        spwanTime = 0f;
        spawnIndex = 0;
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
    public Enemy.EnemyType enemyType;
    public int enemyBaseIndex;

    public WaveEnemyData()
    {
        stage = 0;
        wave = 0;
        spawnTime = 0f;
        enemyType = Enemy.EnemyType.None;
        enemyBaseIndex = 0;
    }
}
