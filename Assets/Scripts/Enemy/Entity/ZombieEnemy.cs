using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ZombieEnemy :Enemy
{
    [SerializeField] float downTime = 2f;
    float downTimeCounter = 0;

    public override void Die(Damageable damageable)
    {
        if (damageable != this.damageable)
        {
            return;
        }
        anim.SetBool("isDead", true);

        nav.isStopped = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        isDead = true;
        OnEnemyDead?.Invoke(this);

        if (isDead)
        {
            Destroy();
        }
        else
        {
            isDead = true;
        }
    }
}
