using Unity.AI.Navigation;

using UnityEditor;

using UnityEngine;
using UnityEngine.AI;

public class NavMeshFromSplatmap :MonoBehaviour
{
    public Terrain terrain;
    public Texture2D splatmap;
    public NavMeshSurface navMeshSurface;

    // NavMeshをベイクするメソッド
    public void BakeNavMesh()
    {
        if (terrain == null || splatmap == null || navMeshSurface == null)
        {
            Debug.LogError("Terrain, Splatmap, または NavMeshSurface が設定されていません。");
            return;
        }

        Debug.Log("NavMeshベイク開始");

        GenerateNavMesh();

        Debug.Log("NavMeshベイク完了");
    }

    private void GenerateNavMesh()
    {
        TerrainData terrainData = terrain.terrainData;
        float[,,] splatMapData = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        for (int x = 0; x < terrainData.alphamapWidth; x++)
        {
            for (int y = 0; y < terrainData.alphamapHeight; y++)
            {
                float r = splatMapData[x, y, 0]; // 赤のチャンネル
                float g = splatMapData[x, y, 1]; // 緑のチャンネル
                // 必要に応じて他のチャンネルも取得可能

                Vector3 worldPosition = terrain.GetPosition() + new Vector3(
                    (x / (float)terrainData.alphamapWidth) * terrainData.size.x,
                    0,
                    (y / (float)terrainData.alphamapHeight) * terrainData.size.z
                );

                // NavMeshエリアを設定
                if (r > 0.5f)
                {
                    // 赤が多い場合、Groundエリアに設定
                    SetNavMeshArea(worldPosition, NavMesh.GetAreaFromName("Ground"));
                }
                else if (g > 0.5f)
                {
                    // 緑が多い場合、Roadエリアに設定
                    SetNavMeshArea(worldPosition, NavMesh.GetAreaFromName("Road"));
                }
            }
        }

        navMeshSurface.BuildNavMesh();
    }

    private void SetNavMeshArea(Vector3 position, int area)
    {
        NavMeshModifierVolume modifier = new GameObject("NavMeshModifier").AddComponent<NavMeshModifierVolume>();
        modifier.center = position;
        modifier.size = new Vector3(1, 1, 1); // 必要に応じてサイズを調整
        modifier.area = area;
    }
}

// カスタムエディターの定義
[CustomEditor(typeof(NavMeshFromSplatmap))]
public class NavMeshFromSplatmapEditor :Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // デフォルトのInspectorを描画

        NavMeshFromSplatmap script = (NavMeshFromSplatmap)target;

        if (GUILayout.Button("Bake NavMesh")) // ボタンがクリックされたとき
        {
            script.BakeNavMesh(); // BakeNavMeshメソッドを呼び出す
        }
    }
}
