using UnityEngine;

[System.Serializable]
public class Crystal
{
    public enum Type
    {
        None = 0,
        Water = 1,
        Fire = 2,
        Wind = 3,
        Thunder = 4,
        Ice = 5,
        Wood = 6,
        Stone = 7,
        Mist = 8,
        Cloud = 9,
        ThunderCloud = 10,
        RainCloud = 11,
        SnowCloud = 12,
        ThunderStorm = 13,
        Typhoon = 14,
    }

    public Type type;
    public Sprite sprite;
    public Material material;
    public float attackPower;
    public float attackSpeed;
    public float attackRate;
    public bool isAreaAttack;
    public float attackRange;
    public float attackArea;

    public Crystal()
    {
        type = Type.None;
        sprite = null;
        material = null;
        attackPower = 0;
        attackSpeed = 0;
        attackRate = 0;
        isAreaAttack = false;
        attackRange = 0;
        attackArea = 0;
    }

    public Crystal(Type type, Sprite sprite)
    {
        this.type = type;
        this.sprite = sprite;
        this.material = null;
        this.attackPower = 0;
        this.attackSpeed = 0;
        this.attackRate = 0;
        this.isAreaAttack = false;
        this.attackRange = 0;
        this.attackArea = 0;
    }
}
