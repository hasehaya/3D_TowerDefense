using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SummonEnemy :Enemy
{
    public EnemyType summonedEnemyType;
    public int summonedCount;
    public float coolTime;

    protected override void Start()
    {
        base.Start();
        var summonAbility = new SummonAbility(this, summonedEnemyType, summonedCount, coolTime, gameObject);
        abilityList.Add(summonAbility);
    }
}
