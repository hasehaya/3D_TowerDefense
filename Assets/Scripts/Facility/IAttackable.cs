/// <summary>
/// 攻撃可能な施設に実装するインターフェース
/// </summary>
public interface IAttackable
{
    public float AttackPower { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackRate { get; set; }
    public bool IsAreaAttack { get; set; }
    public float AttackRange { get; set; }
    public float AttackArea { get; set; }
}
