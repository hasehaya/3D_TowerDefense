using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FlyEnemy :Enemy
{
    Vector3 basePosition;
    bool isNearBase = false;
    const float kBaseDistance = 20;
    protected override void Start()
    {
        base.Start();
        basePosition = StageManager.Instance.GetBase().transform.position;
    }

    protected override void Update()
    {
        base.Update();
        if (isNearBase)
        {
            if (Vector3.Distance(transform.position, basePosition) > 1.0f)
            {
                // ベースに向かって飛行
                Vector3 direction = (basePosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, basePosition) < kBaseDistance)
            {
                GoToBase();
            }
        }
    }


    void GoToBase()
    {
        isNearBase = true;
        nav.enabled = false;
    }
}
