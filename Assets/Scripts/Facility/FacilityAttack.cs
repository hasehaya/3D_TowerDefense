using Unity.VisualScripting;

using UnityEngine;

using static IAttackable;

public class FacilityAttack :Facility
{
    public enum Type
    {
        None = 0,
        Canon = 1,
        Magic = 2,
    }
    public enum AttackType
    {
        None = 0,
        Ground = 1,
        Sky = 2,
        GroundAndSky = 3,
    }
    public Type type;
    public AttackType attackType => _attackType;
    private AttackType _attackType;
    public float AttackPower => attackPower;
    private float attackPower = 0;
    public float AttackSpeed => attackSpeed;
    private float attackSpeed = 0;
    public float AttackRate => attackRate;
    private float attackRate = 0;
    public bool IsAreaAttack => isAreaAttack;
    private bool isAreaAttack = false;
    public float AttackRange => attackRange;
    private float attackRange = 0;
    public float AttackArea => attackArea;
    private float attackArea = 0;
    public Material Material => material;
    private Material material = null;

    private void Awake()
    {
        var status = FacilityManager.Instance.GetFacilityAttackStatus(type);
        _attackType = status.attackType;
        attackPower = status.attackPower;
        attackSpeed = status.attackSpeed;
        attackRate = status.attackRate;
        isAreaAttack = status.isAreaAttack;
        attackRange = status.attackRange;
        attackArea = status.attackArea;
        material = status.material;
    }

    public override void Synthesize(Crystal crystal)
    {
        base.Synthesize(crystal);
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
    }
}

[System.Serializable]
public class FacilityAttackStatus
{
    public FacilityAttack.Type type;
    public FacilityAttack.AttackType attackType;
    public float attackPower;
    public float attackSpeed;
    public float attackRate;
    public bool isAreaAttack;
    public float attackRange;
    public float attackArea;
    public Material material;

    public FacilityAttackStatus()
    {
        type = FacilityAttack.Type.None;
        attackType = FacilityAttack.AttackType.None;
        attackPower = 0;
        attackSpeed = 0;
        attackRate = 0;
        isAreaAttack = false;
        attackRange = 0;
        attackArea = 0;
        material = null;
    }
}
