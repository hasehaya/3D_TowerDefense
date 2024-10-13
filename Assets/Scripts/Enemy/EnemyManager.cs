using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class EnemyManager :MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyManager>();
            }
            return instance;
        }
    }
    [SerializeField] EnemyParameterListEntity enemyStatusListEntity;

    private List<Enemy> enemyList = new List<Enemy>();

    private void Start()
    {
        Enemy.OnEnemyDestroyed += RemoveEnemy;
    }

    public EnemyParameter GetEnemyStatus(EnemyType enemyType)
    {
        foreach (var enemyStatus in enemyStatusListEntity.lists)
        {
            if (enemyStatus.enemyType == enemyType)
            {
                return enemyStatus;
            }
        }
        return null;
    }

    public GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        foreach (var enemyStatus in enemyStatusListEntity.lists)
        {
            if (enemyStatus.enemyType == enemyType)
            {
                return enemyStatus.enemyPrefab;
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
        var enemyObj = Instantiate(enemyPrefab, pos, new Quaternion());
        enemyObj.name = enemyPrefab.name;
        var enemy = enemyObj.GetComponent<Enemy>();
        enemyList.Add(enemy);
    }

    void RemoveEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
    }

    public List<Enemy> GetEnemyList()
    {
        return enemyList;
    }

    public int GetEnemyCount()
    {
        return enemyList.Count;
    }

    bool IsFlyEnemy(EnemyType enemyType)
    {
        var enemyStatus = enemyStatusListEntity.lists.FirstOrDefault(enemyStatus => enemyStatus.enemyType == enemyType);
        var enemy = enemyStatus.enemyPrefab.GetComponent<Enemy>();
        return enemy is FlyEnemy;
    }
}
