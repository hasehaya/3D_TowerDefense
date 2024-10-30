using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Spring :Facility
{
    [SerializeField] float blowOffPower;
    [SerializeField] float coolTime;
    float coolTimeCounter;
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
        coolTimeCounter = coolTime;
    }

    protected override void Update()
    {
        base.Update();
        if (!isInstalled)
        {
            return;
        }
        if (coolTimeCounter > 0)
        {
            coolTimeCounter -= Time.deltaTime;
            return;
        }
        if (enemies.Count == 0)
        {
            return;
        }

        BlowOff();
    }

    void BlowOff()
    {
        coolTimeCounter = coolTime;

        var blowedDirection = Vector3.up * blowOffPower + Vector3.forward * blowOffPower;
        foreach (var enemy in enemies)
        {
            enemy.BlownOff(blowedDirection);
        }
    }
}