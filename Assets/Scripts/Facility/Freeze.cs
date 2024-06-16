using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FacilityFreeze :Facility
{
    [SerializeField] float range;
    [SerializeField] float freezeRate;
    EnemyDetector enemyDetector;
    List<Enemy> enemies { get { return enemyDetector.GetEnemies(); } }

    protected override void Start()
    {
        base.Start();
        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Sphere, range);
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
        foreach (var enemy in enemies)
        {
            enemy.SetFreezeTimeCounter(1, freezeRate);
        }
    }
}
