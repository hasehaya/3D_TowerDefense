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
        SetDestination(GameManager.Instance.GetBase().transform);
    }

    public void SetDestination(Transform destination)
    {
        nav.destination = destination.position;
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
            MoneyManager.Instance.getMoney(1);
        }
    }
}