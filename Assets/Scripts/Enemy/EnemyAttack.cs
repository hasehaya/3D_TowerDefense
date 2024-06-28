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
        damage = (int)enemy.attackPower;
        ct = enemy.attackSpeed;

        var cylinderCol = gameObject.AddComponent<CylinderCollider>();
        cylinderCol.Initialize(enemy.attackRange);
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
