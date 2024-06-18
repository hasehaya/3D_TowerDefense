using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Base :MonoBehaviour, IDamageable
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
}
