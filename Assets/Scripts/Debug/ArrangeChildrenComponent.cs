using UnityEngine;
using UnityEditor;

public class ArrangeChildrenComponent :MonoBehaviour
{
    // Arrangeボタンを押した時に子オブジェクトを整列させるメソッド
    public void ArrangeChildren()
    {
        Undo.RegisterCompleteObjectUndo(transform, "Arrange Children");

        float xOffset = 0f;

        foreach (Transform child in transform)
        {
            // 子オブジェクトの名前の末尾が「PBRDefault」で終わっているか確認
            if (child.name.EndsWith("PBRDefault"))
            {
                // 「PBRDefault」を削除
                child.name = child.name.Substring(0, child.name.Length - "PBRDefault".Length);
            }

            // 整列させる
            child.localPosition = new Vector3(xOffset, child.localPosition.y, child.localPosition.z);
            xOffset -= 3f;
        }
    }
}

[CustomEditor(typeof(ArrangeChildrenComponent))]
public class ArrangeChildrenComponentEditor :Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ArrangeChildrenComponent myScript = (ArrangeChildrenComponent)target;
        if (GUILayout.Button("Arrange Children"))
        {
            myScript.ArrangeChildren();
        }
    }
}
