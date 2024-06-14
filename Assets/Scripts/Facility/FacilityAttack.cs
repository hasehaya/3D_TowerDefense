using Unity.VisualScripting;
using System.Collections.Generic;

using UnityEngine;

using static IAttackable;

public class FacilityAttack :Facility
{
    public enum AttackType
    {
        None = 0,
        Ground = 1,
        Sky = 2,
        GroundAndSky = 3,
    }

    FacilityAttackParameter attackParameter;
    float attackPower;
    float attackSpeed;
    float attackRate;
    bool isAreaAttack;
    float attackRange;
    float attackArea;
    Material material;

    private void Awake()
    {
        var parameter = FacilityManager.Instance.GetFacilityAttackParameter(type);
        attackParameter = parameter;
        attackPower = parameter.attackPower;
        attackSpeed = parameter.attackSpeed;
        attackRate = parameter.attackRate;
        isAreaAttack = parameter.isAreaAttack;
        attackRange = parameter.attackRange;
        attackArea = parameter.attackArea;
    }

    /// <summary>
    /// FacilityAttackのGameObjectをParameterを付けて返す関数
    /// </summary>
    /// <returns>GameObject</returns>
    public static GameObject GenerateFacilityAttack(Type type)
    {
        var facility = GenerateFacility(type);
        var facilityAttack = facility.GetComponent<FacilityAttack>();
        var facilityAttackParameter = FacilityManager.Instance.GetFacilityAttackParameter(type);
        facilityAttack.attackParameter = facilityAttackParameter;
        return facility;
    }

    public override void Synthesize(Crystal crystal)
    {
        var crystalAttack = CrystalManager.Instance.GetCrystalAttack(crystal.type);
        if (crystalAttack == null)
        {
            return;
        }
        attackPower += crystalAttack.attackPower;
        attackSpeed += crystalAttack.attackSpeed;
        attackRate += crystalAttack.attackRate;
        isAreaAttack = crystalAttack.isAreaAttack;
        attackRange += crystalAttack.attackRange;
        attackArea += crystalAttack.attackArea;
        material = crystalAttack.material;
        base.Synthesize(crystal);
    }

    public float GetAttackPower()
    {
        return attackPower;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetAttackRate()
    {
        return attackRate;
    }

    public bool IsAreaAttack()
    {
        return isAreaAttack;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetAttackArea()
    {
        return attackArea;
    }

    public Material GetMaterial()
    {
        return material;
    }

    GameObject bulletPrefab;

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
        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
        facilityAttack = GetComponentInParent<FacilityAttack>();
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        gameObject.layer = LayerMask.NameToLayer("Muzzle");
        SetCapsuleCollider();
    }

    void SetCapsuleCollider()
    {
        capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;
        capsuleCollider.radius = facilityAttack.GetAttackRange();
        capsuleCollider.height = 100;
        Facility.OnFaicilitySynthesized += HandleFacilitySynthesized;
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (!facilityAttack.isInstalled)
        {
            return;
        }
        if (enemies.Count == 0)
        {
            return;
        }

        if (targetEnemy == null)
        {
            targetEnemy = GetMostNearEnemy();
        }

        if (facilityAttack.GetAttackRate() <= coolTimeCounter)
        {
            coolTimeCounter = 0;
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Initialize(facilityAttack, targetEnemy);
        }
        else
        {
            coolTimeCounter += Time.deltaTime;
        }
    }

    Enemy GetMostNearEnemy()
    {
        Enemy mostNearEnemy = null;
        var tempEnemies = new List<Enemy>(enemies);
        foreach (var enemy in tempEnemies)
        {
            if (enemy == null)
            {
                enemies.Remove(enemy);
                continue;
            }

            if (mostNearEnemy == null)
            {
                mostNearEnemy = enemy;
            }
            if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, mostNearEnemy.transform.position))
            {
                mostNearEnemy = enemy;
            }
        }
        return mostNearEnemy;
    }

    private void OnTriggerStay(Collider other)
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

    private void OnTriggerExit(Collider other)
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
        if (enemy == targetEnemy)
        {
            targetEnemy = null;
        }
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    void HandleEnemyDestroyed(Enemy destroyedEnemy)
    {
        if (!enemies.Contains(destroyedEnemy))
        {
            return;
        }
        enemies.Remove(destroyedEnemy);
        if (destroyedEnemy == targetEnemy)
        {
            targetEnemy = null;
        }
    }

    void HandleFacilitySynthesized(Facility facility)
    {
        if (facility != facilityAttack)
        {
            return;
        }
        capsuleCollider.radius = facilityAttack.GetAttackRange();
    }

}

[System.Serializable]
public class FacilityAttackParameter
{
    public Facility.Type type;
    public FacilityAttack.AttackType attackType;
    public float attackPower;
    public float attackSpeed;
    public float attackRate;
    public bool isAreaAttack;
    public float attackRange;
    public float attackArea;
    public GameObject bullet;


    public FacilityAttackParameter()
    {
        type = Facility.Type.Canon;
        attackType = FacilityAttack.AttackType.None;
        attackPower = 0;
        attackSpeed = 0;
        attackRate = 0;
        isAreaAttack = false;
        attackRange = 0;
        attackArea = 0;
        bullet = null;
    }
}

