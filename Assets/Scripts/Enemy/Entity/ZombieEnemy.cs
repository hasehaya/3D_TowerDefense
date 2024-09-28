using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ZombieEnemy :Enemy
{
    public static Action<ZombieEnemy> OnZombieDowned;

    [SerializeField] float downTime = 2f;
    bool isDown = false;

    public override void Die(Damageable damageable)
    {
        if (damageable != this.damageable)
        {
            return;
        }

        if (isDown)
        {
            Destroy(gameObject);
            if (MoneyManager.Instance != null)
            {
                MoneyManager.Instance.AddMoney(money);
            }
        }
        else
        {
            isDown = true;
            OnZombieDowned?.Invoke(this);
        }
    }
}
