using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CutTree :Gimmick, IDamageable, IObstacle
{
    [SerializeField] int hp;
    [SerializeField] float range;
    [SerializeField] Animator anim;
    [SerializeField] Transform target;
    public Damageable damageable { get; set; }
    EnemyDetector enemyDetector;

    public Vector3 Position => target.position;
    public bool IsDestroyed { get; private set; } = false;

    void OnDestroy()
    {
        Damageable.OnDestroyDamageableObject -= DestroyObstacle;
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
        Damageable.OnDestroyDamageableObject += DestroyObstacle;
        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Sphere, range);

        EnemyManager.Instance.RegisterObstacle(this);
    }

    public void TakeDamage(int damage)
    {
        damageable.TakeDamage(damage);
    }

    private void DestroyObstacle(Damageable damageable)
    {
        if (damageable != this.damageable)
        {
            return;
        }
        IsDestroyed = true;
        // 障害物をEnemyManagerから削除
        EnemyManager.Instance.OnObstacleDestroyed(this);
        Destroy(gameObject);
    }
}
