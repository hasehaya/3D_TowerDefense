using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class CrystalBox :MonoBehaviour
{
    private static CrystalBox instance;
    public static CrystalBox Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CrystalBox>();
            }
            return instance;
        }
    }
    [SerializeField] GameObject crystalFramePrefab;
    List<CrystalItem> crystalItems = new List<CrystalItem>();
    List<Crystal> crystals = new List<Crystal>();
    int maxCrystals = 4;

    public CrystalItem draggedCrystal = null;

    private void Start()
    {
        var fire = CrystalManager.Instance.Init(Crystal.Type.Fire);
        var water = CrystalManager.Instance.Init(Crystal.Type.Water);
        for (int i = 0; i < maxCrystals; i++)
        {
            var crystalFrame = Instantiate(crystalFramePrefab, transform);
        }
        crystalItems = GetComponentsInChildren<CrystalItem>().ToList();
        AddCrystal(fire);
        AddCrystal(water);
    }

    public void AddCrystal(Crystal crystal)
    {
        crystals.Add(crystal);
        for (int i = 0; i < crystalItems.Count; i++)
        {
            if (crystalItems[i].type == Crystal.Type.None)
            {
                crystalItems[i].SetCrystal(crystal);
                return;
            }
        }
    }

    public void RemoveCrystal(Crystal crystal)
    {
        crystals.Remove(crystal);
    }

    public void SetMaxCrystals(int maxCrystals)
    {
        this.maxCrystals = maxCrystals;
    }
}
