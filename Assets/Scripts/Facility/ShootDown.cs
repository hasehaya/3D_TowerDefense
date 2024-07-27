using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ShootDown :FacilityAttack
{
    [SerializeField] float shotDownDamage;

    protected override void Awake()
    {
        base.Awake();
        bulletPrefab = Resources.Load<GameObject>("Prefabs/ShotDownBullet");
    }

    protected override void GenerateBullet()
    {
        var bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
        var shotDownBulletComponent = bullet.GetComponent<ShotDownBullet>();
        shotDownBulletComponent.Initialize(this, targetEnemy);
        shotDownBulletComponent.SetShotDownDamage(shotDownDamage);
    }

    protected override Enemy GetMostNearEnemy()
    {
        Enemy mostNearEnemy = null;
        foreach (var enemy in enemies)
        {
            bool isFlyEnemy = enemy is FlyEnemy;
            if (!isFlyEnemy)
            {
                continue;
            }
            var flyEnemy = enemy as FlyEnemy;
            if (!flyEnemy.isFly)
            {
                continue;
            }
            if (mostNearEnemy == null || Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, mostNearEnemy.transform.position))
            {
                mostNearEnemy = enemy;
            }
        }
        return mostNearEnemy;
    }
}
