using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FlyEnemy :Enemy
{
    enum FlyState
    {
        Fly,
        NearBase,
        ShotDown,
        Falling,
        Ground,
        Floating,
    }
    FlyState flyState = FlyState.Fly;
    // 飛行に必要な変数
    Vector3 destination;
    Vector3 basePosition;
    const float kBaseDistance = 20f;
    // 撃墜関係
    public bool isFly { get { return flyState == FlyState.Fly || flyState == FlyState.NearBase; } }
    [SerializeField] float defaultShotDownHp;
    float shotDownHp;
    [SerializeField] float shotDownTime;
    float shotDownCounter = 0;
    Vector3 shotDownPos;

    protected override void Start()
    {
        base.Start();
        nav.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = false;
        shotDownHp = defaultShotDownHp;
        basePosition = StageManager.Instance.GetBase().transform.position;
        EnemyBaseManager.Instance.GetNextDestination(ref enemyNavInfo);
        destination = enemyNavInfo.destination;
    }

    protected override void Update()
    {
        Freeze();
        ExcuteAbilities();

        switch (flyState)
        {
            case FlyState.Fly:
            {
                MoveTowards();

                if (Vector3.Distance(transform.position, basePosition) <= kBaseDistance)
                {
                    flyState = FlyState.NearBase;
                }
                break;
            }

            case FlyState.NearBase:
            {
                Vector3 direction = (basePosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                break;
            }

            case FlyState.ShotDown:
            {
                shotDownPos = transform.position;
                flyState = FlyState.Falling;
                rb.useGravity = true;
                break;
            }

            case FlyState.Falling:
            {
                if (IsGrounded())
                {
                    flyState = FlyState.Ground;
                    rb.useGravity = false;
                }
                break;
            }

            case FlyState.Ground:
            {
                shotDownCounter += Time.deltaTime;
                if (shotDownCounter > shotDownTime)
                {
                    shotDownCounter = 0;
                    flyState = FlyState.Floating;
                }
                break;
            }

            case FlyState.Floating:
            {
                Vector3 direction = (shotDownPos - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                if (Vector3.Distance(transform.position, shotDownPos) <= 0.1f)
                {
                    flyState = FlyState.Fly;
                    shotDownHp = defaultShotDownHp;
                }
                break;
            }
        }
    }

    void MoveTowards()
    {
        Vector3 direction = (destination - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            EnemyBaseManager.Instance.GetNextDestination(ref enemyNavInfo);
            destination = enemyNavInfo.destination;
        }
    }

    public void TakeDamageFromShotDown(float damage, float shotDownDamage)
    {
        TakeDamage(damage);
        if (!isFly)
        {
            return;
        }
        shotDownHp -= shotDownDamage;
        if (shotDownHp <= 0)
        {
            flyState = FlyState.ShotDown;
        }
    }
}
