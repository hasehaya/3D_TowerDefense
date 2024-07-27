using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ShotDownBullet :Bullet
{
    float shotDownDamage;

    protected override void Attack()
    {
        bool isFlyEnemy = enemy is FlyEnemy;
        if (!isFlyEnemy)
        {
            Destroy(gameObject);
            return;
        }
        var flyEnemy = enemy as FlyEnemy;
        flyEnemy.TakeDamageFromShotDown(damage, shotDownDamage);
    }

    public void SetShotDownDamage(float shotDownDamage)
    {
        this.shotDownDamage = shotDownDamage;
    }
}
