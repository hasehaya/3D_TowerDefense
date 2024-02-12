using UnityEngine;
using UnityEngine.AI;

public class Enemy :MonoBehaviour
{
    NavMeshAgent nav;
    [SerializeField] GameObject target;
    [SerializeField] int hp;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        nav.destination = target.transform.position;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}