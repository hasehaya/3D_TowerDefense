using System.Collections.Generic;
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

    public void GetNextDestination(ref EnemyNavInfo navInfo)
    {
        var enemyBase = GetEnemyBase(navInfo);
        Road currentRoad = enemyBase.roads[navInfo.roadIndex];
        // 次のポイントがある場合
        if (navInfo.pointIndex < currentRoad.points.Length)
        {
            navInfo.destination = currentRoad.points[navInfo.pointIndex++];
        }
        // 最後のポイントかつ最後の道の場合
        else if (currentRoad.isLastRoad)
        {
            navInfo.destination = StageManager.Instance.GetBase().transform.position;
        }
        // 次の道に行く
        else
        {
            navInfo.roadIndex++;
            navInfo.pointIndex = 0;
            if (navInfo.roadIndex < enemyBases[navInfo.enemyBaseIndex].roads.Length)
            {
                navInfo.destination = enemyBases[navInfo.enemyBaseIndex].roads[navInfo.roadIndex].points[navInfo.pointIndex++];
            }
            else
            {
                navInfo.destination = Vector3.zero;
            }
        }
    }

    public Vector3 GetSpawnPosition(EnemyNavInfo navInfo)
    {
        EnemyBase enemyBase = GetEnemyBase(navInfo);
        if (enemyBase != null && enemyBase.roads.Length > 0 && enemyBase.roads[0].points.Length > 0)
        {
            return enemyBase.roads[0].points[0];
        }
        return Vector3.zero;
    }

    public EnemyBase GetEnemyBase(EnemyNavInfo navInfo)
    {
        if (navInfo.isFly)
        {
            return enemyBases.FirstOrDefault(enemyBase => enemyBase.isAir && enemyBase.enemyBaseIndex == navInfo.enemyBaseIndex);
        }
        else
        {
            return enemyBases.FirstOrDefault(enemyBase => !enemyBase.isAir && enemyBase.enemyBaseIndex == navInfo.enemyBaseIndex);
        }
    }

    /// <summary>
    /// ルートから外れた際最も近いポイントのルートが属するEnemyBaseからランダムで選択
    /// </summary>
    public void SetMostNearRoad(ref EnemyNavInfo navInfo, Vector3 currentPos)
    {
        float minDistance = float.MaxValue;
        Vector3 closestPoint = Vector3.zero;
        List<(EnemyBase enemyBase, int roadIndex, int pointIndex)> closestPoints = new List<(EnemyBase, int, int)>();

        foreach (var enemyBase in enemyBases)
        {
            for (int i = 0; i < enemyBase.roads.Length; i++)
            {
                Road road = enemyBase.roads[i];
                for (int j = 0; j < road.points.Length; j++)
                {
                    Vector3 point = road.points[j];
                    float distance = Vector3.Distance(currentPos, point);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestPoint = point;
                        closestPoints.Clear();
                        closestPoints.Add((enemyBase, i, j));
                    }
                    else if (distance == minDistance)
                    {
                        closestPoints.Add((enemyBase, i, j));
                    }
                }
            }
        }

        if (closestPoints.Count > 0)
        {
            // 最も近いポイントを持つEnemyBaseの中からランダムに選択
            var selected = closestPoints[Random.Range(0, closestPoints.Count)];
            navInfo.destination = closestPoint;
            navInfo.enemyBaseIndex = selected.enemyBase.enemyBaseIndex;
            navInfo.roadIndex = selected.roadIndex;
            navInfo.pointIndex = selected.pointIndex + 1;

            if (navInfo.pointIndex >= selected.enemyBase.roads[selected.roadIndex].points.Length)
            {
                navInfo.pointIndex = selected.enemyBase.roads[selected.roadIndex].points.Length - 1;
            }
        }
        else
        {
            navInfo.destination = Vector3.zero;
        }
    }
}
