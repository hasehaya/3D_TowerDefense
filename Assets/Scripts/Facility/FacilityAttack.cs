using UnityEngine;

using static IAttackable;

public class FacilityAttack :Facility
{
    public float AttackPower => attackPower;
    private float attackPower = 3;
    public float AttackSpeed => attackSpeed;
    private float attackSpeed = 3;
    public float AttackRate => attackRate;
    private float attackRate = 3;
    public bool IsAreaAttack => isAreaAttack;
    private bool isAreaAttack = false;
    public float AttackRange => attackRange;
    private float attackRange = 3;
    public float AttackArea => attackArea;
    private float attackArea = 3;
    public Material Material => material;
    private Material material;

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
        attackPower += crystalAttack.attackPower;
        attackSpeed += crystalAttack.attackSpeed;
        attackRate += crystalAttack.attackRate;
        isAreaAttack = crystalAttack.isAreaAttack;
        attackRange += crystalAttack.attackRange;
        attackArea += crystalAttack.attackArea;
        material = crystalAttack.material;
    }
}
