using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEnemy : Enemy
{
    [SerializeField] int healValue;
    [SerializeField] float healRange;
    [SerializeField] float coolTime; 

    protected override void Start()
    {
        base.Start();
        var healAbilty = new HealAbilty(healValue, healRange, coolTime, this.gameObject);
        abilityList.Add(healAbilty);
    }

}
