using Unity.AI.Navigation;

using UnityEditor;

using UnityEngine;
using UnityEngine.AI;

public class NavMeshFromSplatmap :MonoBehaviour
{
    public Terrain terrain;
    public Texture2D splatmap;
    public NavMeshSurface navMeshSurface;
    const int roopPixelSize = 5;

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

        // 3ピクセルごとにループを進める
        for (int x = 0; x < terrainData.alphamapWidth; x += roopPixelSize)
        {
            for (int y = 0; y < terrainData.alphamapHeight; y += roopPixelSize)
            {
                float r = splatMapData[x, y, 0]; // 赤のチャンネル
                float g = splatMapData[x, y, 1]; // 緑のチャンネル

                Vector3 worldPosition = terrain.GetPosition() + new Vector3(
                    (x / (float)terrainData.alphamapWidth) * terrainData.size.x,
                    0,
                    (y / (float)terrainData.alphamapHeight) * terrainData.size.z
                );

                if (g > 0.5f)
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
        // 新しいオブジェクトを作成してNavMeshModifierVolumeを追加
        NavMeshModifierVolume modifier = new GameObject("NavMeshModifier").AddComponent<NavMeshModifierVolume>();
        modifier.center = position;
        modifier.size = new Vector3(roopPixelSize, 1, roopPixelSize); // サイズを3ピクセルに合わせて調整
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
