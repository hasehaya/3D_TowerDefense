using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Road :MonoBehaviour
{
    public Vector3[] points;
    public bool isAir;
    public bool isLastRoad;

    private void Start()
    {
        points = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            points[i] = transform.GetChild(i).position;
        }
    }

    public Vector3 GetNextRoadPosition(int pointIndex)
    {
        if (pointIndex + 1 < points.Length)
        {
            return points[pointIndex + 1];
        }
        else
        {
            if (isLastRoad)
            {
                return StageManager.Instance.GetPlayerBasePosition();
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}
