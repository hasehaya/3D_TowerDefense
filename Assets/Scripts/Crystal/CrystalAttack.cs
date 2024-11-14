using UnityEngine;

[System.Serializable]
public class CrystalAttack
{
    public Crystal.Type type;
    public Material material;
    public float attackPower;
    public float attackSpeed;
    public float attackRate;
    public bool isAreaAttack;
    public float attackRange;
    public float attackArea;

    public CrystalAttack()
    {
        type = Crystal.Type.None;
        material = null;
        attackPower = 0;
        attackSpeed = 0;
        attackRate = 0;
        isAreaAttack = true;
        attackRange = 0;
        attackArea = 0;
    }
}