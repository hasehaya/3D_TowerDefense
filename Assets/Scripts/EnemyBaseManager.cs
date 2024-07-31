using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyBaseManager :MonoBehaviour
{
    private static EnemyBaseManager instanse;
    public static EnemyBaseManager Instance
    {
        get
        {
            if (instanse == null)
            {
                instanse = FindObjectOfType<EnemyBaseManager>();
            }
            return instanse;
        }
    }
    public EnemyBase[] enemyBases;

    public Vector3 GetNextDestination(int enemyBaseIndex, ref int roadIndex, ref int pointIndex)
    {
        Road currentRoad = enemyBases[enemyBaseIndex].roads[roadIndex];
        // 次のポイントがある場合
        if (pointIndex < currentRoad.points.Length)
        {
            return currentRoad.points[pointIndex++];
        }
        // 最後のポイントかつ最後の道の場合
        else if (currentRoad.isLastRoad)
        {
            return StageManager.Instance.GetBase().transform.position;
        }
        // 次の道に行く
        else
        {
            roadIndex++;
            pointIndex = 0;
            if (roadIndex < enemyBases[enemyBaseIndex].roads.Length)
            {
                return enemyBases[enemyBaseIndex].roads[roadIndex].points[pointIndex++];
            }
            else
            {
                return Vector3.zero; // End of path
            }
        }
    }
}
