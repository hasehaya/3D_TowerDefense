using UnityEngine;

using static IAttackable;

public class Canon :Facility, IAttackable
{
    public float AttackPower { get; set; }
    [SerializeField] float attackPower;
    public float AttackSpeed { get; set; }
    [SerializeField] float attackSpeed;
    public float AttackRate { get; set; }
    [SerializeField] float attackRate;
    public bool IsAreaAttack { get; set; }
    [SerializeField] bool isAreaAttack;
    public float AttackRange { get; set; }
    [SerializeField] float attackRange;
    public float AttackArea { get; set; }
    [SerializeField] float attackArea;
    public AttackType Type { get; set; }
    [SerializeField] AttackType type;
}