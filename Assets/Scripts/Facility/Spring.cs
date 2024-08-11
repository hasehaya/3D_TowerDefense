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

    void Awake()
    {
        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Box, 3);
    }

    protected override void Start()
    {
        base.Start();
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

    void BlowUp()
    {
        var blowedDirection = Vector3.up * 0.2f + Vector3.forward * 0.2f;
        foreach (var enemy in enemies)
        {
            enemy.BlowedUp(blowedDirection);
        }
    }
}