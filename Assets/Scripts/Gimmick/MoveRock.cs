using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRock :Gimmick
{
    [SerializeField] int damage;
    bool isMoving = false;
    void Start()
    {

    }
    protected override void Update()
    {
        base.Update();
    }

    protected override void Excute()
    {
        isMoving = true;
        NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.MoveRock);
        var anim = GetComponent<Animator>();
        anim.Play("Move1");
    }

    public void finihMoveing()
    {
        isMoving = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isMoving)
        {
            return;
        }
        var collisionGameObj = collision.gameObject;
        // 衝突した相手にタグがEnemy付いているとき
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }

    protected override void ShowNotice()
    {
        NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.MoveRock, WaitPlayerExcute);
    }

    protected override void HideNotice()
    {
        NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.MoveRock);
    }
}
