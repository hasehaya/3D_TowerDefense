using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTree : MonoBehaviour,IDamageable
{
    [SerializeField] int hp;
    [SerializeField] float range;
    bool isNearPlayer = false;
    public Damageable damageable { get; set; }
    EnemyDetector enemyDetector;
    List<Enemy> enemies { get { return enemyDetector.GetEnemies(); } }
    bool isInstalled = false;
    private void Start()
    {
       
    }

    void Update()
    {
        var playerPosition = Player.Instance.transform.position;
        if (Vector3.Distance(transform.position, playerPosition) < 20.0f)
        {
            if(!isNearPlayer)
            {
                NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.CutTree, Cut);
                isNearPlayer = true;
            }
 
        }
        else
        {
            if(isNearPlayer)
            {
                NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.CutTree);
                isNearPlayer = false;
            }
        }

        if(isInstalled)
        {
            Provoke();
        }

    }

    private void Cut()
    {
        NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.CutTree);
        var anim = GetComponent<Animator>();
        anim.Play("Cut");
       
    }

    void BecomTarget()
    {
        damageable = gameObject.AddComponent<Damageable>();
        damageable.Initialize(hp);
        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Capsule, range);
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
            enemy.SetDestination(StageManager.Instance.GetBase().transform);
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
            Vector3 directionToShield = (transform.position - enemy.transform.position).normalized;
            float dotProduct = Vector3.Dot(directionToShield, enemy.transform.forward);
            // 敵が盾の方向を向いている場合（内積が正）
            if (dotProduct > 0)
            {
                var basePos = StageManager.Instance.GetBase().transform.position;
                float distanceToShield = Vector3.Distance(enemy.transform.position, transform.position);
                float distanceToBase = Vector3.Distance(enemy.transform.position, basePos);
                if (distanceToShield < distanceToBase)
                {
                    var destinationPos = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z + 3);
                    var destinationTransForm = transform;
                    destinationTransForm.position += destinationPos;
                    enemy.SetDestination(destinationTransForm);
                }
            }
        }
    }
}
