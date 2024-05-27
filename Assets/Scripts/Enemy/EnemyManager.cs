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

    public void SpawnEnemy(Enemy.EnemyType enemyType, Transform pos)
    {
        var enemyPrefab = GetEnemyPrefab(enemyType);
        Instantiate(enemyPrefab, pos);
    }
}
