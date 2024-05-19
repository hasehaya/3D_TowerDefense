using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy :MonoBehaviour
{
    public delegate void EnemyDestroyed(Enemy enemy);
    public static event EnemyDestroyed OnEnemyDestroyed;

    NavMeshAgent nav;
    Slider slider;
    [SerializeField] GameObject target;
    [SerializeField] float hp;
    [SerializeField] GameObject hpBar;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        SetDestination(GameManager.Instance.GetBase().transform);
        var parent = this.transform;
        var sliderObj = Instantiate(hpBar, parent);
        Vector3 pos = sliderObj.transform.position;
        float height = gameObject.GetComponent<BoxCollider>().size.y;
        pos.y += height + 1;
        sliderObj.transform.position = new Vector3(pos.x, pos.y, pos.z);
;       slider = sliderObj.GetComponentInChildren<Slider>();
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