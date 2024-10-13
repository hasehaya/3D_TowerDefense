using System;
using System.Collections.Generic;

using UnityEngine;

public class EnemyDetector :MonoBehaviour
{
    List<Enemy> enemies = new List<Enemy>();
    Action<Enemy> onEnemyExit;

    public void Initialize(Form form, float range, Action<Enemy> onEnemyExit = null)
    {
        Enemy.OnEnemyDead += HandleEnemyDead;
        this.onEnemyExit = onEnemyExit;
        switch (form)
        {
            case Form.Sphere:
            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = range;
            sphereCollider.isTrigger = true;
            break;
            case Form.Capsule:
            CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.radius = range;
            capsuleCollider.isTrigger = true;
            capsuleCollider.height = 100;
            break;
            case Form.Box:
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(range, range, range);
            boxCollider.isTrigger = true;
            break;
        }
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDead -= HandleEnemyDead;
    }

    void HandleEnemyDead(Enemy destroyedEnemy)
    {
        if (!enemies.Contains(destroyedEnemy))
        {
            return;
        }
        enemies.Remove(destroyedEnemy);
    }

    public List<Enemy> GetEnemies()
    {
        return enemies;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemies.Contains(enemy))
        {
            return;
        }
        enemies.Add(enemy);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemies.Contains(enemy))
        {
            return;
        }
        enemies.Add(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (!enemies.Contains(enemy))
        {
            return;
        }
        enemies.Remove(enemy);
        onEnemyExit?.Invoke(enemy);
    }
}