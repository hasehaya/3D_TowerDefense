using System.Collections.Generic;

using UnityEngine;

public class FacilityAttack :Facility
{
    public enum AttackType
    {
        Ground = 1,
        Sky = 2,
        GroundAndSky = 3,
    }

    [SerializeField] protected Transform muzzlePos;

    [SerializeField] float attackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackRate;
    [SerializeField] bool isAreaAttack;
    [SerializeField] float attackRange;
    [SerializeField] float attackArea;
    [SerializeField] AttackType attackType;
    [SerializeField] Material material;

    EnemyDetector enemyDetector;
    protected List<Enemy> enemies { get { return enemyDetector.GetEnemies(); } }

    protected GameObject bulletPrefab;
    float coolTimeCounter;
    protected Enemy targetEnemy;

    protected override void Awake()
    {
        base.Awake();
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        gameObject.layer = LayerMask.NameToLayer("Muzzle");

        enemyDetector = gameObject.AddComponent<EnemyDetector>();
        enemyDetector.Initialize(Form.Capsule, attackRange, DeleteEnemyFromTargetEnemy);

        Enemy.OnEnemyDead += DeleteEnemyFromTargetEnemy;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Enemy.OnEnemyDead -= DeleteEnemyFromTargetEnemy;
    }

    protected override void Update()
    {
        if (isPaused)
            return;
        base.Update();
        Attack();
    }

    void Attack()
    {
        if (!isInstalled)
        {
            return;
        }

        coolTimeCounter += Time.deltaTime;

        if (enemies.Count == 0)
        {
            return;
        }

        if (targetEnemy == null)
        {
            targetEnemy = GetMostNearEnemy();
            if (targetEnemy == null)
            {
                return;
            }
        }

        if (coolTimeCounter > GetAttackRate())
        {
            coolTimeCounter = 0;
            GenerateBullet();
        }
    }

    protected virtual void GenerateBullet()
    {
        var bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initialize(this, targetEnemy);
    }

    /// <summary>
    /// FacilityAttackのGameObjectをParameterを付けて返す関数
    /// </summary>
    /// <returns>GameObject</returns>
    public static GameObject GenerateFacilityAttack(Type type)
    {
        var facility = GenerateFacility(type);
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

    protected virtual Enemy GetMostNearEnemy()
    {
        Enemy mostNearEnemy = null;
        foreach (var enemy in enemies)
        {
            if (!enemy)
            {
                continue;
            }
            if (enemy.IsDead)
            {
                continue;
            }

            if (!mostNearEnemy)
            {
                mostNearEnemy = enemy;
            }

            var mostNearEnemyDistance = Vector3.Distance(transform.position, mostNearEnemy.transform.position);
            var enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (enemyDistance < mostNearEnemyDistance)
            {
                mostNearEnemy = enemy;
            }
        }
        return mostNearEnemy;
    }

    void DeleteEnemyFromTargetEnemy(Enemy destroyedEnemy)
    {
        if (destroyedEnemy == targetEnemy)
        {
            targetEnemy = null;
        }
    }
}