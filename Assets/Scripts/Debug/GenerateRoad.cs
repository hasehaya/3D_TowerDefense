using Unity.AI.Navigation;

using UnityEditor;

using UnityEngine;
using UnityEngine.AI;

public class GenerateRoad :MonoBehaviour
{
    public Terrain terrain;
    public Texture2D splatmap;
    [SerializeField] GameObject roadParent;
    const int ROOP_PIXEL_SIZE = 1;

    // Method to bake NavMesh
    public void BakeNavMesh()
    {
        RemoveRoads();
        GenerateNavMesh();

        Debug.Log("ロード設置完了");
    }

    void RemoveRoads()
    {
        var children = roadParent.GetComponentsInChildren<BoxCollider>();
        foreach (var child in children)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void GenerateNavMesh()
    {
        TerrainData terrainData = terrain.terrainData;
        float[,,] splatMapData = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        for (int x = 0; x < terrainData.alphamapWidth; x += ROOP_PIXEL_SIZE)
        {
            for (int y = 0; y < terrainData.alphamapHeight; y += ROOP_PIXEL_SIZE)
            {
                float greenChannel = splatMapData[x, y, 1]; // Green channel

                Vector3 worldPosition = terrain.GetPosition() + new Vector3(
                    (x / (float)terrainData.alphamapWidth) * terrainData.size.x,
                    0,
                    (y / (float)terrainData.alphamapHeight) * terrainData.size.z
                );

                if (greenChannel > 0.5f)
                {
                    GenerateRoadObject(worldPosition);
                }
            }
        }
    }

    private void GenerateRoadObject(Vector3 position)
    {
        GameObject roadObject = new GameObject("Road");
        BoxCollider collider = roadObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(ROOP_PIXEL_SIZE, 10, ROOP_PIXEL_SIZE);
        collider.isTrigger = true;
        roadObject.transform.parent = roadParent.transform;
        roadObject.transform.position = position;
        roadObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        roadObject.transform.localScale = Vector3.one;
        roadObject.tag = "Road";
        roadObject.layer = LayerMask.NameToLayer("Road");
    }
}

[CustomEditor(typeof(GenerateRoad))]
public class NavMeshFromSplatmapEditor :Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw default Inspector

        GenerateRoad script = (GenerateRoad)target;

        if (GUILayout.Button("Bake NavMesh")) // When the button is clicked
        {
            script.BakeNavMesh(); // Call the BakeNavMesh method
        }
    }
}
