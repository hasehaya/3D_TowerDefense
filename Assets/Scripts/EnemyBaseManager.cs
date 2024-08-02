using System.Linq;

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
    [SerializeField] GameObject enemyBaseParent;
    public EnemyBase[] enemyBases;

    private void Awake()
    {
        enemyBases = enemyBaseParent.GetComponentsInChildren<EnemyBase>();
    }

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

    public Vector3 GetSpawnPosition(int enemyBaseIndex, bool isFly)
    {
        EnemyBase enemyBase = GetEnemyBase(enemyBaseIndex, isFly);
        if (enemyBase != null && enemyBase.roads.Length > 0 && enemyBase.roads[0].points.Length > 0)
        {
            return enemyBase.roads[0].points[0];
        }
        return Vector3.zero;
    }

    public EnemyBase GetEnemyBase(int enemyBaseIndex, bool isFly)
    {
        if (isFly)
        {
            return enemyBases.FirstOrDefault(enemyBase => enemyBase.isAir && enemyBase.enemyBaseIndex == enemyBaseIndex);
        }
        else
        {
            return enemyBases.FirstOrDefault(enemyBase => !enemyBase.isAir && enemyBase.enemyBaseIndex == enemyBaseIndex);
        }
    }
}
