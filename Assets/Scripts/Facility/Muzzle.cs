using System.Collections.Generic;

using UnityEngine;

public class Muzzle :MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;

    FacilityAttack facilityAttack;
    float coolTimeCounter;
    List<Enemy> enemies = new List<Enemy>();
    Enemy targetEnemy;


    private void Start()
    {
        facilityAttack = GetComponentInParent<FacilityAttack>();
        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (!facilityAttack.isInstalled)
        {
            return;
        }
        if (enemies.Count == 0)
        {
            return;
        }
        print(enemies.Count);
        if (targetEnemy == null)
        {
            targetEnemy = GetMostNearEnemy();
        }

        if (10/facilityAttack.AttackRate <= coolTimeCounter)
        {
            coolTimeCounter = 0;
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Initialize(facilityAttack,targetEnemy);
        }
        else
        {
            coolTimeCounter += Time.deltaTime;
        }
    }

    Enemy GetMostNearEnemy()
    {
        Enemy mostNearEnemy = null;
        var tempEnemies = new List<Enemy>(enemies);
        foreach (var enemy in tempEnemies)
        {
            if(enemy == null)
            {
                enemies.Remove(enemy);
            }

            if (mostNearEnemy == null)
            {
                mostNearEnemy = enemy;
            }
            if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, mostNearEnemy.transform.position))
            {
                mostNearEnemy = enemy;
            }
        }
        return mostNearEnemy;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!facilityAttack.isInstalled)
        {
            return;
        }
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        var enemy = other.gameObject.GetComponent<Enemy>();
        if (targetEnemy == null)
        {
            targetEnemy = enemy;
        }
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!facilityAttack.isInstalled)
        {
            return;
        }
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy == targetEnemy)
        {
            targetEnemy = null;
        }
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    void HandleEnemyDestroyed(Enemy destroyedEnemy)
    {
        if (!enemies.Contains(destroyedEnemy))
        {
            return;
        }
        enemies.Remove(destroyedEnemy);
        if (destroyedEnemy == targetEnemy)
        {
            targetEnemy = null;
        }
    }
}
