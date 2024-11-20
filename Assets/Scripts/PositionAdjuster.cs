using UnityEditor;

using UnityEngine;

public class PositionAdjuster :MonoBehaviour
{
    public int groundLayer = 8;
    public float adjustHeight = 0.4f;
    public float rayDistance = 50f;

    // 子オブジェクトの位置を一括で調整
    public void AdjustChildrenPositions()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                AdjustPosition(grandChild);
            }
        }
    }

    private void AdjustPosition(Transform obj)
    {
        RaycastHit hit;
        LayerMask layerMask = 1 << groundLayer;

        // 上方向にRayを飛ばす
        if (Physics.Raycast(obj.position, Vector3.up, out hit, rayDistance, layerMask))
        {
            SetPosition(obj, hit.point);
            return;
        }

        // 下方向にRayを飛ばす
        if (Physics.Raycast(obj.position, Vector3.down, out hit, rayDistance, layerMask))
        {
            SetPosition(obj, hit.point);
            return;
        }
    }

    private void SetPosition(Transform obj, Vector3 hitPoint)
    {
        obj.position = hitPoint + Vector3.up * adjustHeight;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PositionAdjuster))]
public class PositionAdjusterEditor :Editor
{
    public override void OnInspectorGUI()
    {
        // ベースのInspector GUIを描画
        DrawDefaultInspector();

        PositionAdjuster positionAdjuster = (PositionAdjuster)target;

        // ボタンを追加
        if (GUILayout.Button("Adjust Children Positions"))
        {
            positionAdjuster.AdjustChildrenPositions();
        }
    }
}
#endif