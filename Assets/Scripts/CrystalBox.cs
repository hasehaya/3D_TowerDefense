using System.Collections.Generic;

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
    List<Crystal> crystals = new List<Crystal>();
    int maxCrystals = 10;

    public void AddCrystal(Crystal crystal)
    {
        crystals.Add(crystal);
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
