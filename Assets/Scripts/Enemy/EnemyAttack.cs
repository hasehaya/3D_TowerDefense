using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyAttack :MonoBehaviour
{
    Base home;
    Rigidbody rb;
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] float ct;

    float coolTimeCounter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetHome(Base home)
    {
        this.home = home;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Home")
        {
            if (ct <= coolTimeCounter)
            {
                home = other.gameObject.GetComponent<Base>();
                home.TakeDamage(damage);
                coolTimeCounter = 0;
            }
            else
            {
                coolTimeCounter += Time.deltaTime;
            }

        }
    }

    void Update()
    {
        if (!home)
        {
            return;
        }
        if (!rb)
        {
            return;
        }
        rb.velocity = (home.transform.position - transform.position).normalized * speed;
    }
}
