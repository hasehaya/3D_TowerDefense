﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Mine :Facility
{
    [SerializeField] int damage;
    ParticleSystem explosionEffect;
    CapsuleCollider capsuleCollider;
    bool isExploded = false;

    protected override void Start()
    {
        base.Start();
        explosionEffect = Resources.Load<ParticleSystem>("Prefabs/Explosion");
        SetCapsuleCollider();
    }

    void SetCapsuleCollider()
    {
        capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;
        capsuleCollider.radius = 5;
        capsuleCollider.height = 100;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isInstalled)
        {
            return;
        }
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        if (isExploded)
        {
            return;
        }
        isExploded = true;
        AreaDamage.Create(transform.position, Form.Sphere, damage, 10, 1, 0);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
