using UnityEngine;

using static IAttackable;

public class FacilityAttack :Facility
{
    public float AttackPower => attackPower;
    private float attackPower;
    public float AttackSpeed => attackSpeed;
    private float attackSpeed;
    public float AttackRate => attackRate;
    private float attackRate;
    public bool IsAreaAttack => isAreaAttack;
    private bool isAreaAttack;
    public float AttackRange => attackRange;
    private float attackRange;
    public float AttackArea => attackArea;
    private float attackArea;

    public AttackType type;
    public enum AttackType
    {
        Ground,
        Sky,
        GroundAndSky
    }

    public override void Synthesize(Crystal crystal)
    {
        base.Synthesize(crystal);
        var crystalAttack = CrystalManager.Instance.GetCrystalAttack(crystal.type);
        if (crystalAttack == null)
        {
            return;
        }
        attackPower = crystalAttack.attackPower;
        attackSpeed = crystalAttack.attackSpeed;
        attackRate = crystalAttack.attackRate;
        isAreaAttack = crystalAttack.isAreaAttack;
        attackRange = crystalAttack.attackRange;
        attackArea = crystalAttack.attackArea;
    }
}
