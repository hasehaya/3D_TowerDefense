using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class EnemyBaseManager :MonoBehaviour
{
    private static EnemyBaseManager instanse;
    public static EnemyBaseManager Instance
    {
        get
        {
            if (instanse == null)
            {
                instanse = FindObjectOfType<EnemyBaseManager>();
            }
            return instanse;
        }
    }

    [SerializeField] GameObject enemyBaseParent;
    EnemyBase[] enemyBases;

    private void Awake()
    {
        enemyBases = enemyBaseParent.GetComponentsInChildren<EnemyBase>();
    }

    public EnemyBase GetEnemyBase(int enemyBaseIndex, bool isFly)
    {
        return enemyBases.FirstOrDefault(enemyBase => enemyBase.enemyBaseIndex == enemyBaseIndex && enemyBase.isAir == isFly);
    }
}
