using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu]
public class CrystalAttackListEntity :ScriptableObject
{
    public List<CrystalAttack> crystalAttacks = new List<CrystalAttack>();
}

[System.Serializable]
public class CrystalAttack
{
    public Crystal.Type type;
    public Material material;
    public float attack;
    public float attackRate;
    public float attackSpeed;
    public bool isAreaAttack;
    public float attackRange;
}