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
    NavMeshAgent nav;
    // HP関係
    Slider slider;
    [SerializeField] GameObject hpBar;
    // 敵の種類
    public enum EnemyType
    {
        None = 0,
        Slime = 1,
        Turtle = 2,
    }
    [SerializeField] EnemyType enemyType;
    // ステータス
    GameObject enemyPrefab;
    float hp = 10;
    float speed = 2f;
    float attackPower = 1.0f;
    float attackSpeed = 1.0f;
    float attackRange = 1.0f;
    Attribute attribute = Attribute.None;

    void Start()
    {
        SetStatus();
        SetNavigation();
        SetHpSlider();
        AddRigidBody();
        gameObject.tag = "Enemy";
    }

    void SetStatus()
    {
        var status = EnemyManager.Instance.GetEnemyStatus(enemyType);
        enemyPrefab = status.enemyPrefab;
        hp = status.hp;
        speed = status.speed;
        attackPower = status.attackPower;
        attackSpeed = status.attackSpeed;
        attackRange = status.attackRange;
        attribute = status.attribute;
    }

    void SetNavigation()
    {
        nav = GetComponent<NavMeshAgent>();
        SetDestination(GameManager.Instance.GetBase().transform);
        nav.speed = speed;
    }

    void SetHpSlider()
    {
        var sliderObj = Instantiate(hpBar, transform);
        Vector3 pos = sliderObj.transform.position;
        float height = GetComponent<BoxCollider>().size.y;
        pos.y += height + 1;
        sliderObj.transform.position = pos;
        slider = sliderObj.GetComponentInChildren<Slider>();
        slider.maxValue = hp;
        slider.value = hp;
    }

    void AddRigidBody()
    {
        gameObject.AddComponent<Rigidbody>();
    }

    public void SetDestination(Transform destination)
    {
        nav.destination = destination.position;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        slider.value -= damage;
        if (hp <= 0)
        {
            OnEnemyDestroyed?.Invoke(this);
            MoneyManager.Instance.getMoney(1);
            Destroy(gameObject);
        }
    }
}


[System.Serializable]
public class EnemyStatus
{
    public Enemy.EnemyType enemyType;
    public GameObject enemyPrefab;
    public float hp;
    public float speed;
    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public Attribute attribute;

    public EnemyStatus()
    {
        enemyType = Enemy.EnemyType.None;
        enemyPrefab = null;
        hp = 0;
        speed = 0;
        attackPower = 0;
        attackSpeed = 0;
        attackRange = 0;
        attribute = Attribute.None;
    }
}