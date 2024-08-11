using UnityEngine;

public struct EnemyNavInfo
{
    public Vector3 destination;
    public int enemyBaseIndex;
    public int roadIndex;
    public int pointIndex;
    public bool isFly;

    public EnemyNavInfo(int enemyBaseIndex, bool isFly)
    {
        this.enemyBaseIndex = enemyBaseIndex;
        roadIndex = 0;
        pointIndex = 0;
        destination = Vector3.zero;
        this.isFly = isFly;
    }
}