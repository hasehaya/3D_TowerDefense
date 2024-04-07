/// <summary>
/// �U���\�Ȏ{��
/// </summary>
public interface IAttackable
{
    public float AttackPower { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackRate { get; set; }
    public bool IsAreaAttack { get; set; }
    public float AttackRange { get; set; }
    public float AttackArea { get; set; }
    public AttackType Type { get; set; }
    public enum AttackType { Ground, Sky, GroundAndSky }
}
