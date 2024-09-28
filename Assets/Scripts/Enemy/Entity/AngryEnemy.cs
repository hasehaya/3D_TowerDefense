using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AngryEnemy :Enemy
{
    [SerializeField] float speedUpRate = 1.5f;
    [SerializeField] float speedUpTime = 1f;
    float speedUpTimeCounter = 0;
    bool isAngry = false;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        speedUpTimeCounter = speedUpTime;
    }

    protected override void Update()
    {
        base.Update();
        Angry();
    }

    void Angry()
    {
        if (isAngry)
        {
            if (speedUpTimeCounter > 0)
            {
                speedUpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isAngry = false;
                nav.speed = speed;
            }
        }
        else
        {
            if (speedUpTimeCounter > 0)
            {
                isAngry = true;
                nav.speed = speed * speedUpRate;
            }
        }
    }
}
