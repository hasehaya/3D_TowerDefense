using UnityEngine;

// このスクリプトは、円柱形のカスタムコライダーを作成し、MeshColliderを使用して物理的な当たり判定を提供します。
// また、Unityエディタ上で円柱の形状を緑色の線で表示します。

[AddComponentMenu("Physics/Custom Cylinder Collider")]
public class CustomCylinderCollider :MonoBehaviour
{
    // 円柱の高さを設定する
    public float height = 2.0f;
    // 円柱の半径を設定する
    public float radius = 0.5f;
    // 円柱の周囲を構成するセグメント数を設定する
    public int segments = 20;
    // 円柱の位置オフセットを設定する
    public Vector3 offset = Vector3.zero;

    private Mesh mesh;

    // 初期化時に円柱を作成する
    void Start()
    {
        CreateCylinder();
    }

    // 円柱のメッシュを作成し、MeshColliderに設定するメソッド
    void CreateCylinder()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }

        meshFilter.mesh = mesh = new Mesh();
        mesh.name = "CylinderCollider";

        int vertexCount = (segments + 1) * 2 + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[segments * 12];

        float angle = 0f;
        float angleStep = 360f / segments;

        // 上面と底面の中心点
        vertices[vertexCount - 2] = new Vector3(0, 0, 0) + offset;
        vertices[vertexCount - 1] = new Vector3(0, height, 0) + offset;

        for (int i = 0; i <= segments; i++)
        {
            float radian = Mathf.Deg2Rad * angle;
            float cos = Mathf.Cos(radian) * radius;
            float sin = Mathf.Sin(radian) * radius;

            // 下部の頂点
            vertices[i] = new Vector3(cos, 0, sin) + offset;
            // 上部の頂点
            vertices[i + segments + 1] = new Vector3(cos, height, sin) + offset;

            if (i < segments)
            {
                int baseIndex = i * 6;
                int capBaseIndex = segments * 6 + i * 6;

                // 側面の三角形
                triangles[baseIndex] = i;
                triangles[baseIndex + 1] = i + segments + 1;
                triangles[baseIndex + 2] = i + 1;

                triangles[baseIndex + 3] = i + 1;
                triangles[baseIndex + 4] = i + segments + 1;
                triangles[baseIndex + 5] = i + segments + 2;

                // 底面の三角形
                triangles[capBaseIndex] = vertexCount - 2;
                triangles[capBaseIndex + 1] = i;
                triangles[capBaseIndex + 2] = (i + 1) % segments;

                // 上面の三角形（反時計回りに設定）
                triangles[capBaseIndex + 3] = vertexCount - 1;
                triangles[capBaseIndex + 5] = i + segments + 1;
                triangles[capBaseIndex + 4] = ((i + 1) % segments) + segments + 1;
            }

            angle += angleStep;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
    }

    // Unityエディタ上で円柱の形状を描画するメソッド
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 bottomCenter = transform.position + offset;
        Vector3 topCenter = bottomCenter + Vector3.up * height;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 bottomVertex = new Vector3(x, 0, z) + bottomCenter;
            Vector3 topVertex = new Vector3(x, height, z) + bottomCenter;

            // 下部から上部への線を描画する
            Gizmos.DrawLine(bottomVertex, topVertex);

            // 隣接するセグメント間の線を描画する
            if (i < segments)
            {
                float nextAngle = (i + 1) * Mathf.PI * 2 / segments;
                float nextX = Mathf.Cos(nextAngle) * radius;
                float nextZ = Mathf.Sin(nextAngle) * radius;
                Vector3 nextBottomVertex = new Vector3(nextX, 0, nextZ) + bottomCenter;
                Vector3 nextTopVertex = new Vector3(nextX, height, nextZ) + bottomCenter;

                Gizmos.DrawLine(bottomVertex, nextBottomVertex);
                Gizmos.DrawLine(topVertex, nextTopVertex);
            }
        }

        // 下部の線を描画する
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            float nextAngle = (i + 1) * Mathf.PI * 2 / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            float nextX = Mathf.Cos(nextAngle) * radius;
            float nextZ = Mathf.Sin(nextAngle) * radius;
            Vector3 bottomVertex = new Vector3(x, 0, z) + bottomCenter;
            Vector3 nextBottomVertex = new Vector3(nextX, 0, nextZ) + bottomCenter;

            Gizmos.DrawLine(bottomVertex, nextBottomVertex);
        }

        // 上部の線を描画する
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            float nextAngle = (i + 1) * Mathf.PI * 2 / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            float nextX = Mathf.Cos(nextAngle) * radius;
            float nextZ = Mathf.Sin(nextAngle) * radius;
            Vector3 topVertex = new Vector3(x, height, z) + bottomCenter;
            Vector3 nextTopVertex = new Vector3(nextX, height, nextZ) + bottomCenter;

            Gizmos.DrawLine(topVertex, nextTopVertex);
        }

        // 底面の中心から各頂点への線を描画する
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 bottomVertex = new Vector3(x, 0, z) + bottomCenter;

            Gizmos.DrawLine(bottomCenter, bottomVertex);
        }

        // 上面の中心から各頂点への線を描画する
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 topVertex = new Vector3(x, height, z) + bottomCenter;

            Gizmos.DrawLine(topCenter, topVertex);
        }
    }
}
