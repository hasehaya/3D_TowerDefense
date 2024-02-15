using UnityEngine;

public class Crystal 
{
    public enum Type
    {
        Water,
        Fire,
        Wind,
        Sunder,
        Ice,
        Wood,
        Stone,

    }

    public Type type;
    public Sprite sprite;

    public Crystal(Type type, Sprite sprite)
    {
        this.type = type;
        this.sprite = sprite;
    }
}
