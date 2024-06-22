using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Shield :Facility, IDamageable
{
    [SerializeField] int hp;
    [SerializeField] float range;
    public Damageable damageable { get; set; }
    EnemyDetector enemyDetector;
    List<Enemy> enemies { get { return enemyDetector.GetEnemies(); } }
    Vector3 BasePos { get { return GameManager.Instance.GetBase().transform.position; } }

    protected override void Start()
    {
        base.Start();
        damageable = gameObject.AddComponent<Damageable>();
        damageable.Initialize(hp);
        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Sphere, range);
    }

    protected override void Update()
    {
        base.Update();
        Provoke();
    }

    private void OnDestroy()
    {
        foreach (var enemy in enemies)
        {
            enemy.SetDestination(GameManager.Instance.GetBase().transform);
        }
    }

    void Provoke()
    {
        if (!isInstalled)
        {
            return;
        }
        foreach (var enemy in enemies)
        {
            if (enemy is FlyEnemy)
            {
                continue;
            }
            Vector3 directionToShield = (transform.position - enemy.transform.position).normalized;
            float dotProduct = Vector3.Dot(directionToShield, enemy.transform.forward);
            // 敵が盾の方向を向いている場合（内積が正）
            if (dotProduct > 0)
            {
                float distanceToShield = Vector3.Distance(enemy.transform.position, transform.position);
                float distanceToBase = Vector3.Distance(enemy.transform.position, BasePos);
                if (distanceToShield < distanceToBase)
                {
                    enemy.SetDestination(transform);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        damageable.TakeDamage(damage);
    }
}
