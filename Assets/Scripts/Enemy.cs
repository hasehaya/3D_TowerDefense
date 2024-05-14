using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy :MonoBehaviour
{
    // 破壊された際に呼ばれるイベント
    public delegate void EnemyDestroyed(Enemy enemy);
    public static event EnemyDestroyed OnEnemyDestroyed;

    NavMeshAgent nav;
    // HP関係
    Slider slider;
    [SerializeField] GameObject hpBar;
    // ステータス
    float hp = 10;
    float speed = 1.0f;
    float attackPower = 1.0f;
    float attackSpeed = 1.0f;
    float attackRange = 1.0f;
    Attribute attribute = Attribute.None;

    void Start()
    {
        SetNavigation();
        SetHpSlider();
    }

    void SetNavigation()
    {
        nav = GetComponent<NavMeshAgent>();
        SetDestination(GameManager.Instance.GetBase().transform);
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
            Destroy(gameObject);
        }
    }
}