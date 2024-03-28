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

    public Crystal(Type type, Sprite sprite)
    {
        this.type = type;
        this.sprite = sprite;
    }
}
