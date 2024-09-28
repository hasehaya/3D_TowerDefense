using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerBase :MonoBehaviour, IDamageable
{
    public Damageable damageable { get; set; }
    void Start()
    {
        damageable = gameObject.AddComponent<Damageable>();
        damageable.Initialize(100);
    }

    public void TakeDamage(int damage)
    {
        damageable.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;
        }
        var enemy = other.gameObject.GetComponent<Enemy>();
        enemy.EnterBase();
    }
}
