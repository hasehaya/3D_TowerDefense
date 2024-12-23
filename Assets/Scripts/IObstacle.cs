using UnityEngine;

public interface IObstacle
{
    Vector3 Position { get; }
    bool IsDestroyed { get; }
}
