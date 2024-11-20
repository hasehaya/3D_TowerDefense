using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CutTree :Gimmick, IDamageable
{
    [SerializeField] int hp;
    [SerializeField] float range;
    [SerializeField] Animator anim;
    [SerializeField] Transform target;
    public Damageable damageable { get; set; }
    EnemyDetector enemyDetector;
    List<Enemy> enemies { get { return enemyDetector.GetEnemies(); } }
    bool isInstalled = false;

    void OnDestroy()
    {
        Damageable.OnDestroyDamageableObject -= Destroy;
    }

    protected override void Update()
    {
        base.Update();
        if (isInstalled)
        {
            Provoke();
        }

    }
    protected override void ShowNotice()
    {
        NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.CutTree, WaitPlayerExcute);
    }

    protected override void HideNotice()
    {
        NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.CutTree);
    }

    protected override void Excute()
    {
        NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.CutTree);
        anim.Play("Cut");
        StartCoroutine(WaitForAnimation("Cut"));
    }

    private IEnumerator WaitForAnimation(string animationName)
    {
        yield return null;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        while (stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        }

        BecomTarget();
    }

    void BecomTarget()
    {
        damageable = gameObject.AddComponent<Damageable>();
        damageable.Initialize(hp);
        damageable.SetHpBarPosition(target.position);
        Damageable.OnDestroyDamageableObject += Destroy;
        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Sphere, range);
        isInstalled = true;
    }

    public void TakeDamage(int damage)
    {
        damageable.TakeDamage(damage);
    }

    private void Destroy()
    {
        foreach (var enemy in enemies)
        {
            enemy.SetDestination(StageManager.Instance.GetPlayerBasePosition());
        }
        Destroy(gameObject);
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
            Vector3 directionToTree = (target.position - enemy.transform.position).normalized;
            float dotProduct = Vector3.Dot(directionToTree, enemy.transform.forward);
            // 敵が盾の方向を向いている場合（内積が正）
            if (dotProduct > 0)
            {
                var basePos = StageManager.Instance.GetPlayerBasePosition();
                float distanceToTree = Vector3.Distance(enemy.transform.position, target.position);
                float distanceToBase = Vector3.Distance(enemy.transform.position, basePos);
                if (distanceToTree < distanceToBase)
                {
                    var destinationPos = target.position;

                    enemy.SetDestination(destinationPos);
                }
            }
        }
    }
}
