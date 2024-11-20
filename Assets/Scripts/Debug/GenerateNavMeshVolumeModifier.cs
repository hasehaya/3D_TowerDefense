using System.Collections.Generic;

using Unity.AI.Navigation;

using UnityEditor;

using UnityEngine;
using UnityEngine.AI;

public class GenerateNavMeshVolumeModifier :MonoBehaviour
{
    [SerializeField] Terrain terrain;
    [SerializeField] Texture2D splatmap;
    [SerializeField] GameObject roadParent;
    const int ROOP_PIXEL_SIZE = 1;

    public void BakeNavMesh()
    {
        Debug.Log("NavMeshModifierVolume設置");
        RemoveModifier();
        GenerateNavMesh();
        Debug.Log("NavMeshModifierVolume設置完了");
    }

    void RemoveModifier()
    {
        var children = roadParent.GetComponentsInChildren<NavMeshModifierVolume>();
        foreach (var child in children)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void GenerateNavMesh()
    {
        TerrainData terrainData = terrain.terrainData;
        float[,,] splatMapData = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        // エリア情報をまとめて格納するリスト
        List<Vector3> roadPositions = new List<Vector3>();
        List<Vector3> groundPositions = new List<Vector3>();

        // 3ピクセルごとにループを進める
        for (int x = 50; x < terrainData.alphamapWidth - 150; x += ROOP_PIXEL_SIZE)
        {
            for (int y = 150; y < terrainData.alphamapHeight - 100; y += ROOP_PIXEL_SIZE)
            {
                float g = splatMapData[x, y, 1];

                Vector3 worldPosition = new Vector3(
                    (x / (float)terrainData.alphamapWidth) * terrainData.size.x,
                    0,
                    (y / (float)terrainData.alphamapHeight) * terrainData.size.z
                );

                if (g > 0.5f)
                {
                    roadPositions.Add(worldPosition);
                }
                else
                {
                    groundPositions.Add(worldPosition);
                }
            }
        }

        // 道路エリアを設定
        SetNavMeshArea(roadPositions, NavMesh.GetAreaFromName("Road"));
        // 地面エリアを設定
        //SetNavMeshArea(groundPositions, NavMesh.GetAreaFromName("Ground"));
    }

    private void SetNavMeshArea(List<Vector3> positions, int area)
    {
        foreach (var position in positions)
        {
            NavMeshModifierVolume modifier = new GameObject("NavMeshModifier").AddComponent<NavMeshModifierVolume>();
            modifier.transform.parent = roadParent.transform;
            // X180度回転,Y-90度回転
            modifier.transform.position = new Vector3(position.z, 0, position.x);
            modifier.size = new Vector3(ROOP_PIXEL_SIZE, 80, ROOP_PIXEL_SIZE);
            modifier.area = area;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GenerateNavMeshVolumeModifier))]
public class NavMeshFromSplatmapEditor :Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenerateNavMeshVolumeModifier script = (GenerateNavMeshVolumeModifier)target;

        if (GUILayout.Button("Bake NavMesh"))
        {
            script.BakeNavMesh();
        }
    }
}
#endif
