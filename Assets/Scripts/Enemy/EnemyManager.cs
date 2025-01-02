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

    // 障害物リスト追加
    private List<IObstacle> obstacles = new List<IObstacle>();

    EnemyManager()
    {
        enemyParameterArray = ScriptableObjectManager.Instance.GetEnemyParameterArray();
        Enemy.OnEnemyDestroyed += RemoveEnemy;
    }

    ~EnemyManager()
    {
        Enemy.OnEnemyDestroyed -= RemoveEnemy;
    }

    public static void DestroyInstance()
    {
        instance = null;
    }

    public void RegisterObstacle(IObstacle obstacle)
    {
        if (!obstacles.Contains(obstacle))
        {
            obstacles.Add(obstacle);
        }
    }

    public List<IObstacle> GetObstacles()
    {
        return obstacles;
    }

    public void OnObstacleDestroyed(IObstacle obstacle)
    {
        if (obstacles.Contains(obstacle))
        {
            obstacles.Remove(obstacle);
        }

        // 障害物破壊時に生存中の全Enemyのターゲットを再設定
        foreach (var enemy in livingEnemyList)
        {
            if (enemy != null && !enemy.IsDead)
            {
                enemy.UpdateTarget();
            }
        }
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
        var enemyObj = Object.Instantiate(enemyPrefab, pos, Quaternion.identity);
        enemyObj.name = enemyPrefab.name;

        var enemy = enemyObj.GetComponent<Enemy>();
        var basePos = StageManager.Instance.GetPlayerBasePosition();
        enemy.transform.LookAt(new Vector3(basePos.x, enemy.transform.position.y, basePos.z));

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
