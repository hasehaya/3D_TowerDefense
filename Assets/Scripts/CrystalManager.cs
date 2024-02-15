using UnityEngine;

public class CrystalManager :MonoBehaviour
{
    private static CrystalManager instance;
    public static CrystalManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CrystalManager>();
            }
            return instance;
        }
    }
    [SerializeField] GameObject crystalPrefab;
    CrystalListEntity crystalListEntity;

    public GameObject Spawn(Crystal.Type type, Vector3 position)
    {
        foreach (var crystal in crystalListEntity.crystals)
        {
            if (crystal.type == type)
            {
                return Instantiate(crystalPrefab, position, Quaternion.identity);
            }
        }
        return null;
    }

    public Sprite GetSprite(Crystal.Type type)
    {
        foreach (var crystal in crystalListEntity.crystals)
        {
            if (crystal.type == type)
            {
                return crystal.sprite;
            }
        }
        return null;
    }
}
