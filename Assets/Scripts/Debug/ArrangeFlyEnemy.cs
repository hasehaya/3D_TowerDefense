using UnityEditor;

using UnityEngine;
using UnityEngine.AI;

public class ArrangeFlyEnemy :MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }
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

            child.tag = "Enemy";
            child.gameObject.layer = LayerMask.NameToLayer("Enemy");

            var damageable = child.gameObject.GetComponent<Damageable>();
            if (!damageable)
            {
                damageable = child.gameObject.AddComponent<Damageable>();
            }

            var name = child.name;
            var enemy = child.gameObject.GetComponent<Enemy>();
            if (System.Enum.TryParse<EnemyType>(name, out var enemyType))
            {
                enemy.EnemyType = enemyType;
            }
        }
    }
}

[CustomEditor(typeof(ArrangeFlyEnemy))]
public class ArrangeChildrenComponentEditor :Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ArrangeFlyEnemy myScript = (ArrangeFlyEnemy)target;
        if (GUILayout.Button("Arrange Children"))
        {
            myScript.ArrangeChildren();
        }
    }
}
