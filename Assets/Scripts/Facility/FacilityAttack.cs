using System.Collections.Generic;

using UnityEngine;

public class FacilityAttack :Facility
{
    public enum AttackType
    {
        None = 0,
        Ground = 1,
        Sky = 2,
        GroundAndSky = 3,
    }

    [SerializeField] Transform muzzlePos;

    FacilityAttackParameter attackParameter;
    float attackPower;
    float attackSpeed;
    float attackRate;
    bool isAreaAttack;
    float attackRange;
    float attackArea;
    Material material;

    EnemyDetector enemyDetector;
    List<Enemy> enemies { get { return enemyDetector.GetEnemies(); } }

    GameObject bulletPrefab;
    float coolTimeCounter;
    Enemy targetEnemy;

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


        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        gameObject.layer = LayerMask.NameToLayer("Muzzle");

        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Capsule, attackRange);

        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    protected override void Update()
    {
        base.Update();
        Attack();
    }

    void Attack()
    {
        if (!isInstalled)
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

        if (GetAttackRate() <= coolTimeCounter)
        {
            coolTimeCounter = 0;
            var bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Initialize(this, targetEnemy);
        }
        else
        {
            coolTimeCounter += Time.deltaTime;
        }
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

    Enemy GetMostNearEnemy()
    {
        Enemy mostNearEnemy = null;
        foreach (var enemy in enemies)
        {
            if (mostNearEnemy == null || Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, mostNearEnemy.transform.position))
            {
                mostNearEnemy = enemy;
            }
        }
        return mostNearEnemy;
    }

    void HandleEnemyDestroyed(Enemy destroyedEnemy)
    {
        if (enemies.Contains(destroyedEnemy))
        {
            if (destroyedEnemy == targetEnemy)
            {
                targetEnemy = null;
            }
        }
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

