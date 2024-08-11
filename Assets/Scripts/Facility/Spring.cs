using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Spring :Facility
{
    [SerializeField] float coolTime;
    float coolCounter;
    BoxCollider boxCollider;
    protected List<Enemy> enemies { get { return enemyDetector.GetEnemies(); } }
    EnemyDetector enemyDetector;

    protected virtual void Awake()
    {
        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Box, 3);
    }

    protected override void Start()
    {
        base.Start();
        SetBoxCollider();
        coolCounter = coolTime;
    }

    protected override void Update()
    {
        base.Update();
        if (!isInstalled)
        {
            return;
        }
        if (coolCounter > 0)
        {
            coolCounter -= Time.deltaTime;
            return;
        }
        if (enemies.Count == 0)
        {
            return;
        }

        BlowUp();
    }

    void SetBoxCollider()
    {
        boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(3, 2, 3);
    }

    void BlowUp()
    {
        var blowedDirection = Vector3.up * 10 + Vector3.forward * 10;
        foreach (var enemy in enemies)
        {
            enemy.BlowedUp(blowedDirection);
        }
    }
}