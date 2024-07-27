using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[System.Serializable]
public class Enemy :MonoBehaviour
{
    // 破壊された際に呼ばれるイベント
    public delegate void EnemyDestroyed(Enemy enemy);
    public static event EnemyDestroyed OnEnemyDestroyed;

    // ナビゲーション
    protected NavMeshAgent nav;
    Transform currentDestination = null;
    protected Rigidbody rb;
    // HP関係
    Damageable damageable;
    // 敵の種類
    public enum EnemyType
    {
        None = 0,
        Slime = 1,
        Turtle = 2,
        Quick = 3,
        Healer = 4,
        Tank = 5,
        Summon = 6,
        Boss = 7,
        Fly = 8,
        Multiply = 9,
    }
    [SerializeField] EnemyType enemyType;
    // ステータス
    GameObject enemyPrefab;
    protected float hp => damageable.CurrentHp;
    protected float maxHp = 10;
    protected float speed = 2f;
    int money = 0;
    public float attackPower = 1.0f;
    public float attackSpeed = 1.0f;
    public float attackRange = 1.0f;
    Attribute attribute = Attribute.None;

    //アビリティーリスト
    protected List<IAbility> abilityList = new List<IAbility>();
    //状態異常
    //フリーズから抜けてから何秒間継続するか
    float freezeTimeCounter = 0;
    //フリーズの強度
    float freezeRate = 0;

    protected virtual void Start()
    {
        damageable = gameObject.AddComponent<Damageable>();
        SetStatus();
        SetNavigation();
        AddRigidBody();
        AddEnemyAttack();
        gameObject.tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    protected virtual void Update()
    {
        Freeze();
        ExcuteAbilities();
    }

    protected bool IsGrounded()
    {
        RaycastHit hit;
        int layerToTarget = 8;
        LayerMask layerMask = 1 << layerToTarget;
        var position = transform.position + Vector3.up * 2;
        Physics.Raycast(position, Vector3.down, out hit, 3.5f, layerMask);
        return hit.collider != null;
    }

    void Freeze()
    {
        if (freezeTimeCounter > 0)
        {
            freezeTimeCounter -= Time.deltaTime;
            if (freezeTimeCounter <= 0)
            {
                nav.speed = speed;
            }
        }
    }

    void ExcuteAbilities()
    {
        foreach (var ability in abilityList)
        {
            ability.counter += Time.deltaTime;
            if (ability.counter >= ability.coolTime)
            {
                ability.Excute();
                ability.counter = 0;
            }
        }
    }

    protected virtual void OnDestroy()
    {
        OnEnemyDestroyed?.Invoke(this);
        MoneyManager.Instance.AddMoney(money);
    }

    void SetStatus()
    {
        var status = EnemyManager.Instance.GetEnemyStatus(enemyType);
        enemyPrefab = status.enemyPrefab;
        damageable.Initialize(status.hp);
        maxHp = hp;
        speed = status.speed;
        money = status.money;
        attackPower = status.attackPower;
        attackSpeed = status.attackSpeed;
        attackRange = status.attackRange;
        attribute = status.attribute;
    }

    void SetNavigation()
    {
        nav = GetComponent<NavMeshAgent>();
        SetDestination(StageManager.Instance.GetBase().transform);
        nav.speed = speed;
    }

    void AddRigidBody()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = true;
        rb.freezeRotation = true;
    }

    public void SetDestination(Transform destination)
    {
        if (currentDestination == destination)
        {
            return;
        }
        currentDestination = destination;
        nav.destination = destination.position;
    }

    public void setNavPosition(Vector3 pos)
    {
        nav.destination = pos;
    }

    void AddEnemyAttack()
    {
        var children = new GameObject("EnemyAttack");
        children.transform.parent = transform;
        children.transform.localPosition = Vector3.zero;
        var enemyAttack = children.AddComponent<EnemyAttack>();
        enemyAttack.Initialize(this);
    }

    public void TakeDamage(float damage)
    {
        damageable.TakeDamage(damage);
    }

    /// <summary>
    /// フリーズさせる関数
    /// </summary>
    public void SetFreezeTimeCounter(float freezeTime, float freezeRate)
    {
        freezeTimeCounter = freezeTime;
        if (freezeRate > this.freezeRate)
        {
            this.freezeRate = freezeRate;
            nav.speed = speed * freezeRate;
        }
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }
}


[System.Serializable]
public class EnemyParameter
{
    public Enemy.EnemyType enemyType;
    public GameObject enemyPrefab;
    public float hp;
    public float speed;
    public int money;
    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public Attribute attribute;

    public EnemyParameter()
    {
        enemyType = Enemy.EnemyType.None;
        enemyPrefab = null;
        hp = 0;
        speed = 0;
        money = 0;
        attackPower = 0;
        attackSpeed = 0;
        attackRange = 0;
        attribute = Attribute.None;
    }
}