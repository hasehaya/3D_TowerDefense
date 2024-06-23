using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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

    public EnemyParameter GetEnemyStatus(Enemy.EnemyType enemyType)
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

    public GameObject GetEnemyPrefab(Enemy.EnemyType enemyType)
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

    public void SpawnEnemy(Enemy.EnemyType enemyType, Vector3 pos)
    {
        var enemyPrefab = GetEnemyPrefab(enemyType);
        var enemyObj = Instantiate(enemyPrefab, pos, new Quaternion());
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
}
