using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ShootDown :FacilityAttack
{
    [SerializeField] float shootDownDamage;

    protected override void Awake()
    {
        base.Awake();
        bulletPrefab = Resources.Load<GameObject>("Prefabs/ShootDownBullet");
    }

    protected override void Start()
    {
        base.Start();
        FlyEnemy.OnEnemyShootDown += HandleEnemyShootDown;
    }

    private void OnDestroy()
    {
        FlyEnemy.OnEnemyShootDown -= HandleEnemyShootDown;
    }

    void HandleEnemyShootDown(FlyEnemy enemy)
    {
        targetEnemy = GetMostNearEnemy();
    }

    protected override void GenerateBullet()
    {
        var bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
        var shootDownBulletComponent = bullet.GetComponent<ShootDownBullet>();
        shootDownBulletComponent.Initialize(this, targetEnemy);
        shootDownBulletComponent.SetShootDownDamage(shootDownDamage);
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
            if (!flyEnemy.IsFlying)
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
