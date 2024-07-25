using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRock : MonoBehaviour
{
    [SerializeField] int damage;
    bool isNearPlayer = false;
    bool isMoving = false;
    void Start()
    {
        
    }
    void Update()
    {
        var playerPosition = Player.Instance.transform.position;
        if (Vector3.Distance(transform.position, playerPosition) < 20.0f)
        {
            if (!isNearPlayer)
            {
                NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.MoveRock, Move);
                isNearPlayer = true;
            }

        }
        else
        {
            if (isNearPlayer)
            {
                NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.MoveRock);
                isNearPlayer = false;
            }
        }
    }

    private void Move()
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
        if(!isMoving)
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
}
