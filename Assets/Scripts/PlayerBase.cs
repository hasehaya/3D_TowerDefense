using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerBase :MonoBehaviour, IDamageable
{
    public static Action<int, int> OnChangeBaseHp;

    int maxHp;
    int currentHp;
    public Damageable damageable { get; set; }
    void Start()
    {
        damageable = gameObject.AddComponent<Damageable>();
        damageable.Initialize(100);

        maxHp = 20;
        currentHp = maxHp;
        OnChangeBaseHp?.Invoke(currentHp, maxHp);
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
        //TODO: enemyに応じてHPの減少量を変更する
        currentHp--;
        if (currentHp <= 0)
        {
            StageManager.Instance.GameOver();
        }
        OnChangeBaseHp?.Invoke(currentHp, maxHp);
    }
}
