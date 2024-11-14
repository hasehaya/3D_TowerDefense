using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class EnemyManager
{
    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyManager();
            }
            return instance;
        }
    }
    EnemyParameter[] enemyParameterArray;

    private List<Enemy> livingEnemyList = new List<Enemy>();

    EnemyManager()
    {
        enemyParameterArray = ScriptableObjectManager.Instance.GetEnemyParameterArray();
        Enemy.OnEnemyDestroyed += RemoveEnemy;
    }

    ~EnemyManager()
    {
        Enemy.OnEnemyDestroyed -= RemoveEnemy;
    }

    public EnemyParameter GetEnemyStatus(EnemyType enemyType)
    {
        foreach (var enemyParameter in enemyParameterArray)
        {
            if (enemyParameter.enemyType == enemyType)
            {
                return enemyParameter;
            }
        }
        return null;
    }

    public GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        foreach (var enemyParameter in enemyParameterArray)
        {
            if (enemyParameter.enemyType == enemyType)
            {
                return enemyParameter.enemyPrefab;
            }
        }
        return null;
    }

    public void SpawnEnemy(EnemyType enemyType, int enemyBaseIndex)
    {
        var isFly = IsFlyEnemy(enemyType);
        var enemyBase = EnemyBaseManager.Instance.GetEnemyBase(enemyBaseIndex, isFly);
        var spawnPos = enemyBase.transform.position;
        SpawnEnemy(enemyType, spawnPos);
    }

    public void SpawnEnemy(EnemyType enemyType, Vector3 pos)
    {
        var enemyPrefab = GetEnemyPrefab(enemyType);
        var enemyObj = Object.Instantiate(enemyPrefab, pos, new Quaternion());
        enemyObj.name = enemyPrefab.name;
        var enemy = enemyObj.GetComponent<Enemy>();
        livingEnemyList.Add(enemy);
    }

    void RemoveEnemy(Enemy enemy)
    {
        livingEnemyList.Remove(enemy);
    }

    public List<Enemy> GetEnemyList()
    {
        return livingEnemyList;
    }

    public int GetEnemyCount()
    {
        return livingEnemyList.Count;
    }

    bool IsFlyEnemy(EnemyType enemyType)
    {
        var enemyParameter = enemyParameterArray.FirstOrDefault(enemyParameter => enemyParameter.enemyType == enemyType);
        var enemy = enemyParameter.enemyPrefab.GetComponent<Enemy>();
        return enemy is FlyEnemy;
    }
}
