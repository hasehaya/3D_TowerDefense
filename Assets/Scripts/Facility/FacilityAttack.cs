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

    [SerializeField] float attackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackRate;
    [SerializeField] bool isAreaAttack;
    [SerializeField] float attackRange;
    [SerializeField] float attackArea;
    [SerializeField] Material material;

    EnemyDetector enemyDetector;
    List<Enemy> enemies { get { return enemyDetector.GetEnemies(); } }

    protected GameObject bulletPrefab;
    float coolTimeCounter;
    Enemy targetEnemy;

    private void Awake()
    {
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