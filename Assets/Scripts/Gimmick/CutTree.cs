using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CutTree :Gimmick, IDamageable
{
    [SerializeField] int hp;
    [SerializeField] float range;
    [SerializeField] Animator anim;
    public Damageable damageable { get; set; }
    EnemyDetector enemyDetector;
    List<Enemy> enemies { get { return enemyDetector.GetEnemies(); } }
    bool isInstalled = false;

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

        RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        string methodName = "BecomTarget";

        foreach (var clip in ac.animationClips)
        {
            if (clip.name == "Cut")
            {
                var finishEvent = new AnimationEvent();
                finishEvent.functionName = methodName;
                finishEvent.time = clip.length;
                clip.AddEvent(finishEvent);
                break; // Exit the loop after adding the event to the "Cut" clip
            }
        }
    }

    void BecomTarget()
    {
        damageable = gameObject.AddComponent<Damageable>();
        damageable.Initialize(hp);
        var child = transform.Find("Child").gameObject;
        var worldPos = child.gameObject.transform.position;
        damageable.SetHpBarPosition(worldPos);
        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Sphere, range);
        isInstalled = true;
    }

    public void TakeDamage(int damage)
    {
        damageable.TakeDamage(damage);
    }

    private void OnDestroy()
    {
        foreach (var enemy in enemies)
        {
            enemy.SetDestination(StageManager.Instance.GetPlayerBasePosition());
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
            Vector3 directionToTree = (transform.position - enemy.transform.position).normalized;
            float dotProduct = Vector3.Dot(directionToTree, enemy.transform.forward);
            // 敵が盾の方向を向いている場合（内積が正）
            if (dotProduct > 0)
            {
                var basePos = StageManager.Instance.GetPlayerBasePosition();
                float distanceToTree = Vector3.Distance(enemy.transform.position, transform.position);
                float distanceToBase = Vector3.Distance(enemy.transform.position, basePos);
                if (distanceToTree < distanceToBase)
                {
                    var child = transform.Find("Child").gameObject;
                    var destinationPos = child.gameObject.transform.position;

                    enemy.SetDestination(destinationPos);
                }
            }
        }
    }
}
