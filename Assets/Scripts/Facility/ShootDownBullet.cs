using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ShootDownBullet :Bullet
{
    float shootDownDamage;

    protected override void Attack()
    {
        bool isFlyEnemy = enemy is FlyEnemy;
        if (!isFlyEnemy)
        {
            Destroy(gameObject);
            return;
        }
        var flyEnemy = enemy as FlyEnemy;
        flyEnemy.TakeDamageFromShotDown(damage, shootDownDamage);
    }

    public void SetShotDownDamage(float shotDownDamage)
    {
        this.shootDownDamage = shotDownDamage;
    }
}
