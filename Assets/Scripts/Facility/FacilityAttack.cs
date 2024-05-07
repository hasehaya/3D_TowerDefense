using UnityEngine;

using static IAttackable;

public class FacilityAttack :Facility
{
    float attackPower;
    float attackSpeed;
    float attackRate;
    bool isAreaAttack;
    float attackRange;
    float attackArea;

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
