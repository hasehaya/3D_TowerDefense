using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SummonEnemy :Enemy
{
    [SerializeField] EnemyType summonedEnemyType;
    [SerializeField] int summonedCount;
    [SerializeField] float coolTime;

    protected override void Start()
    {
        base.Start();
        var summonAbility = new SummonAbility(summonedEnemyType, summonedCount, coolTime, this.gameObject);
        abilityList.Add(summonAbility);
    }
}
