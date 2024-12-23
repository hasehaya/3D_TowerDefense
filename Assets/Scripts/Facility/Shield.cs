using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Shield :Facility, IDamageable, IObstacle
{
    [SerializeField] int hp;
    [SerializeField] float range;
    public Damageable damageable { get; set; }
    EnemyDetector enemyDetector;

    public Vector3 Position => transform.position;
    public bool IsDestroyed { get; private set; } = false;

    protected override void Start()
    {
        base.Start();
        damageable = gameObject.AddComponent<Damageable>();
        damageable.Initialize(hp);
        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Sphere, range);

        Damageable.OnDestroyDamageableObject += HandleDestroyObject;

        // EnemyManagerに登録
        EnemyManager.Instance.RegisterObstacle(this);
    }

    private void OnDestroy()
    {
        Damageable.OnDestroyDamageableObject -= HandleDestroyObject;
        if (!IsDestroyed)
        {
            // 破壊として扱う
            IsDestroyed = true;
            EnemyManager.Instance.OnObstacleDestroyed(this);
        }
    }

    void HandleDestroyObject(Damageable damageable)
    {
        if (damageable == this.damageable)
        {
            IsDestroyed = true;
            EnemyManager.Instance.OnObstacleDestroyed(this);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        damageable.TakeDamage(damage);
    }
}

