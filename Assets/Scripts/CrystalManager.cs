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
    [SerializeField] CrystalListEntity crystalListEntity;
    [SerializeField] GameObject crystalPositionParent;
    Transform[] crystalPositions;

    private void Start()
    {
        crystalPositions = crystalPositionParent.GetComponentsInChildren<Transform>();
        foreach (var crystalPosition in crystalPositions)
        {
            Spawn(Crystal.Type.Fire, crystalPosition.position);
        }
    }

    public Crystal Init(Crystal.Type type)
    {
        foreach (var crystal in crystalListEntity.crystals)
        {
            if (crystal.type == type)
            {
                return crystal;
            }
        }
        return null;
    }

    public void Spawn(Crystal.Type type, Vector3 position)
    {
        foreach (var crystal in crystalListEntity.crystals)
        {
            if (crystal.type == type)
            {
                var crystalField = Instantiate(crystalPrefab, position, Quaternion.identity);
                crystalField.GetComponent<CrystalField>().SetCrystal(crystal);
            }
        }
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
