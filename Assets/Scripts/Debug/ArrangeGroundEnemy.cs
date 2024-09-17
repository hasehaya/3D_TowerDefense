using UnityEditor;

using UnityEngine;
using UnityEngine.AI;

public class ArrangeGroundEnemy :MonoBehaviour
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

            child.tag = "Enemy";
            child.gameObject.layer = LayerMask.NameToLayer("Enemy");

            var cylinder = child.gameObject.GetComponent<CylinderCollider>();
            var nav = child.GetComponent<NavMeshAgent>();
            if (!nav)
            {
                nav = child.gameObject.AddComponent<NavMeshAgent>();
            }
            nav.height = cylinder.height;
            nav.radius = cylinder.radius;

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

[CustomEditor(typeof(ArrangeGroundEnemy))]
public class ArrangeGroundEnemyEditor :Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ArrangeGroundEnemy myScript = (ArrangeGroundEnemy)target;
        if (GUILayout.Button("Arrange Children"))
        {
            myScript.ArrangeChildren();
        }
    }
}
