using UnityEngine;

public class WaveManager :MonoBehaviour
{
    [SerializeField] Transform[] enemyBases;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] WaveDataListEntity waveDataListEntity;
    float spwanTime = 0f;
    int index = 0;

    private void Update()
    {
        spwanTime += Time.deltaTime;
        while (index < waveDataListEntity.lists.Length && waveDataListEntity.lists[index].spawnTime <= spwanTime)
        {
            var waveData = waveDataListEntity.lists[index];
            EnemyManager.Instance.SpawnEnemy(waveData.enemyType, GetEnemyBase(waveData.enemyBaseIndex));
            index++;
        }
    }

    public Transform GetEnemyBase(int index)
    {
        return enemyBases[index];
    }
}

[System.Serializable]
public class WaveData
{
    public int index;
    public float spawnTime;
    public Enemy.EnemyType enemyType;
    public int enemyBaseIndex;

    public WaveData()
    {
        index = 0;
        spawnTime = 0f;
        enemyType = Enemy.EnemyType.None;
        enemyBaseIndex = 0;
    }
}
