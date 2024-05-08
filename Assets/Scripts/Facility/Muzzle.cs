using System.Collections.Generic;

using UnityEngine;

public class Muzzle :MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;

    FacilityAttack facility;
    float coolTimeCounter;
    List<Enemy> enemies = new List<Enemy>();
    Enemy targetEnemy;


    private void Start()
    {
        facility = GetComponentInParent<FacilityAttack>();
    }

    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (!facility.isInstalled)
        {
            return;
        }
        if (enemies.Count == 0)
        {
            return;
        }

        if (targetEnemy == null)
        {
            targetEnemy = GetMostNearEnemy();
        }

        if (facility.AttackRate <= coolTimeCounter)
        {
            coolTimeCounter = 0;
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetEnemy(targetEnemy);
        }
        else
        {
            coolTimeCounter += Time.deltaTime;
        }
    }

    Enemy GetMostNearEnemy()
    {
        Enemy mostNearEnemy = null;
        foreach (var enemy in enemies)
        {
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

    private void OnTriggerEnter(Collider other)
    {
        if (!facility.isInstalled)
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
        if (!facility.isInstalled)
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
}
