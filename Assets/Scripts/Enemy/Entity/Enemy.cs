using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
[DisallowMultipleComponent]
public class Enemy :MonoBehaviour
{
    // 破壊された際に呼ばれるイベント
    public static Action<Enemy> OnEnemyDestroyed;

    // ナビゲーション
    [HideInInspector] public NavMeshAgent nav;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;
    // HP関係
    protected Damageable damageable;
    // 敵の種類
    [SerializeField] EnemyType enemyType;
    // ステータス
    protected float maxHp = 10;
    protected float speed = 2f;
    protected int money = 0;
    protected float attackPower = 1.0f;
    protected float attackSpeed = 1.0f;
    protected float attackRange = 1.0f;
    protected Attribute attribute = Attribute.Normal;

    // アビリティーリスト
    protected List<IAbility> abilityList = new List<IAbility>();
    // 状態異常
    // フリーズから抜けてから何秒間継続するか
    private float freezeTimeCounter = 0;
    // フリーズの強度
    private float freezeRate = 0;

    // StateMachineのための状態
    private IEnemyState currentState;

    // NavMeshエリアのインデックス
    [HideInInspector] public int roadArea;
    [HideInInspector] public int groundArea;

    // プロパティ
    public int RoadArea => roadArea;
    public int GroundArea => groundArea;
    //TODO: 後で修正
    public EnemyType EnemyType { get { return enemyType; } set { enemyType = value; } }
    public float Speed => speed;
    public float AttackPower => attackPower;
    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;
    public Attribute Attribute => attribute;

    // 現在のHP（読み取り専用）
    public float CurrentHp => damageable.CurrentHp;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        damageable = GetComponent<Damageable>();
        anim = GetComponent<Animator>();
        Damageable.OnDestroyDamageableObject += Die;
        SetStatus();
        AddEnemyAttack();
        SetNavMeshAgent();

        // NavMeshエリアのインデックスを取得
        roadArea = NavMesh.GetAreaFromName("Road");
        groundArea = NavMesh.GetAreaFromName("Ground");

        // 初期状態を設定
        StartState();
    }

    protected virtual void StartState()
    {
        TransitionToState(new WalkingOnRoadState(this));
    }

    private void SetNavMeshAgent()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        int layerToTarget = LayerMask.NameToLayer("Ground"); // レイヤー名を使用
        LayerMask layerMask = 1 << layerToTarget;
        var position = transform.position + Vector3.up * 2;
        if (Physics.Raycast(position, Vector3.down, out hit, 3.5f, layerMask))
        {
            return hit.collider != null;
        }
        return false;
    }

    protected virtual void Update()
    {
        Freeze();
        ExecuteAbilities();
        UpdateState();
    }

    protected virtual void UpdateState()
    {
        currentState.UpdateState();
    }

    public void TransitionToState(IEnemyState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    protected void Freeze()
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

    protected void ExecuteAbilities()
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
        Damageable.OnDestroyDamageableObject -= Die;
        OnEnemyDestroyed?.Invoke(this);
    }

    private void SetStatus()
    {
        var status = EnemyManager.Instance.GetEnemyStatus(enemyType);
        damageable.Initialize(status.hp);
        maxHp = status.hp;
        speed = status.speed * 30 / 100;
        money = status.money;
        attackPower = status.attackPower;
        attackSpeed = status.attackSpeed;
        attackRange = status.attackRange;
        attribute = status.attribute;
    }

    public void SetDestination(Transform destination)
    {
        nav.destination = destination.position;
    }

    public void SetDestination(Vector3 pos)
    {
        nav.destination = pos;
    }

    private void AddEnemyAttack()
    {
        var children = new GameObject("EnemyAttack");
        children.transform.parent = transform;
        children.transform.localPosition = Vector3.zero;
        var enemyAttack = children.AddComponent<EnemyAttack>();
        enemyAttack.Initialize(this);
    }

    /// <summary>
    /// ダメージ処理のみ、ヒールのためには使わない
    /// </summary>
    public virtual void TakeDamage(float damage)
    {
        damageable.TakeDamage(damage);
    }

    /// <summary>
    /// 中身でマイナスに変換しているため引数は正の値でよい
    /// </summary>
    public void Heal(float healAmount)
    {
        damageable.TakeDamage(-healAmount);
    }

    public virtual void Die(Damageable damageable)
    {
        if (damageable != this.damageable)
        {
            return;
        }

        Destroy(gameObject);
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(money);
        }
    }

    public void EnterBase()
    {
        Destroy(gameObject);
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

    public void BlownOff(Vector3 blowedDirection)
    {
        // 吹き飛ばされた際の処理を状態遷移で管理
        TransitionToState(new BlownOffState(this, blowedDirection));
    }
}


[System.Serializable]
public class EnemyParameter
{
    public EnemyType enemyType;
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
        enemyType = EnemyType.None;
        enemyPrefab = null;
        hp = 0;
        speed = 0;
        money = 0;
        attackPower = 0;
        attackSpeed = 0;
        attackRange = 0;
        attribute = Attribute.Normal;
    }
}
