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
    [SerializeField] EnemyStatusListEntity enemyStatusListEntity;

    public EnemyStatus GetEnemyStatus(Enemy.EnemyType enemyType)
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
}
