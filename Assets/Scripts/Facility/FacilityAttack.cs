using Unity.VisualScripting;

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
    FacilityAttackParameter _attackParamater;
    float _attackPower;
    float _attackSpeed;
    float _attackRate;
    bool _isAreaAttack;
    float _attackRange;
    float _attackArea;
    Material _material;

    private void Awake()
    {
        var status = FacilityManager.Instance.GetFacilityAttackParameter(type);
        _attackParamater = status;
    }

    public static FacilityAttack Init(Type type)
    {
        var facilityAttack = new FacilityAttack();
        var parameter = FacilityManager.Instance.GetFacilityAttackParameter(type);
        facilityAttack._attackParamater = parameter;
        return facilityAttack;
    }

    public override void Synthesize(Crystal crystal)
    {
        base.Synthesize(crystal);
        var crystalAttack = CrystalManager.Instance.GetCrystalAttack(crystal.type);
        if (crystalAttack == null)
        {
            return;
        }
        _attackPower += crystalAttack.attackPower;
        _attackSpeed += crystalAttack.attackSpeed;
        _attackRate += crystalAttack.attackRate;
        _isAreaAttack = crystalAttack.isAreaAttack;
        _attackRange += crystalAttack.attackRange;
        _attackArea += crystalAttack.attackArea;
        _material = crystalAttack.material;
    }

    public float GetAttackPower()
    {
        return _attackPower;
    }

    public float GetAttackSpeed()
    {
        return _attackSpeed;
    }

    public float GetAttackRate()
    {
        return _attackRate;
    }

    public bool IsAreaAttack()
    {
        return _isAreaAttack;
    }

    public float GetAttackRange()
    {
        return _attackRange;
    }

    public float GetAttackArea()
    {
        return _attackArea;
    }

    public Material GetMaterial()
    {
        return _material;
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

