using System.Collections.Generic;

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
}

[CreateAssetMenu]
public class CrystalAttackListEntity :ScriptableObject
{
    public List<CrystalAttack> crystalAttacks = new List<CrystalAttack>();
}