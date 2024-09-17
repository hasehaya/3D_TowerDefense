using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyAttack :MonoBehaviour
{
    Enemy enemy;
    int damage;
    float ct;

    float coolTimeCounter;

    public void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
        damage = (int)enemy.AttackPower;
        ct = enemy.AttackSpeed;

        var cylinderCol = gameObject.AddComponent<CylinderCollider>();
        cylinderCol.Initialize(enemy.AttackRange);
    }

    private void OnTriggerStay(Collider other)
    {
        bool isDamageableObj = other.CompareTag("Home") || other.CompareTag("Shield") || other.CompareTag("Tree");
        if (!isDamageableObj)
        {
            return;
        }

        if (ct <= coolTimeCounter)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable == null)
            {
                return;
            }
            damageable.TakeDamage(damage);
            coolTimeCounter = 0;
        }
        else
        {
            coolTimeCounter += Time.deltaTime;
        }
    }


}
