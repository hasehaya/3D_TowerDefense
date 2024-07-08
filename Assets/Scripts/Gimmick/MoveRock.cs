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
        if (Vector3.Distance(transform.position, playerPosition) < 5.0f)
        {
            if (!isNearPlayer)
            {
                NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.MoveRock, Cut);
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

    private void MoveRock()
    {
        isMoving = true;
        NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.MoveRock);
        var anim = GetComponent<Animator>();
        anim.Play("Move");
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!isMoving)
        {
            return;
        }
        // 衝突した相手にタグがEnemy付いているとき
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }
}
