using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Mine :MonoBehaviour
{
    ParticleSystem explosionEffect;

    FacilityAttack facilityAttack;
    // あたり判定
    CapsuleCollider capsuleCollider;
    // クールタイムを数える変数
    float coolTimeCounter;
    // 敵のリスト
    List<Enemy> enemies = new List<Enemy>();
    Enemy targetEnemy;


    private void Start()
    {
        facilityAttack = GetComponentInParent<FacilityAttack>();
        explosionEffect = Resources.Load<ParticleSystem>("Prefabs/Bullet");
        gameObject.layer = LayerMask.NameToLayer("Muzzle");
        SetCapsuleCollider();
    }

    void SetCapsuleCollider()
    {
        capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;
        capsuleCollider.radius = facilityAttack.GetAttackRange();
        capsuleCollider.height = 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!facilityAttack.isInstalled)
        {
            return;
        }
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        var enemy = other.gameObject.GetComponent<Enemy>();
        if (targetEnemy == null)
        {
            targetEnemy = enemy;
        }
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }
}
