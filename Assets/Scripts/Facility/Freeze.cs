using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityFreeze : Facility
{
    [SerializeField] float range;
    [SerializeField] float freezeRate;

    // あたり判定
    CapsuleCollider capsuleCollider;
    // 敵のリスト
    List<Enemy> enemies = new();

    protected override void Start()
    {
        base.Start();
        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
        SetCapsuleCollider();
    }

    void SetCapsuleCollider()
    {
        capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;
        capsuleCollider.radius = range;
        capsuleCollider.height = 100;
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    protected override void Update()
    {
        base.Update();
        Freeze();
    }

    void Freeze()
    {
        if (!isInstalled)
        {
            return;
        }
        foreach(var enemy in enemies)
        {
            enemy.SetFreezeTimeCounter(1, freezeRate);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isInstalled)
        {
            return;
        }
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        var enemy = other.gameObject.GetComponent<Enemy>();
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isInstalled)
        {
            return;
        }
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        var enemy = other.gameObject.GetComponent<Enemy>();
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
    }
}
