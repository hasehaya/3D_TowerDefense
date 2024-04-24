using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] GameObject crystalItemPrefab;
    [SerializeField] RectTransform crystalFrameParent;
    List<CrystalFrame> crystalFrames = new List<CrystalFrame>();
    List<Crystal> crystals = new List<Crystal>();
    int maxCrystals = 4;

    private void Start()
    {
        var fire = CrystalManager.Instance.Init(Crystal.Type.Fire);
        var water = CrystalManager.Instance.Init(Crystal.Type.Water);
        for (int i = 0; i < maxCrystals; i++)
        {
            Instantiate(crystalFramePrefab, crystalFrameParent);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(crystalFrameParent);
        crystalFrames = GetComponentsInChildren<CrystalFrame>().ToList();
        AddCrystal(fire);
        AddCrystal(water);
    }

    public void AddCrystal(Crystal crystal)
    {
        crystals.Add(crystal);
        for (int i = 0; i < crystalFrames.Count; i++)
        {
            if (crystalFrames[i].GetCrystal() == null)
            {
                var crystalItem = Instantiate(crystalItemPrefab, transform);
                var crystalImage = crystalItem.GetComponentInChildren<Image>();
                crystalFrames[i].SetFrame(crystal, crystalImage);
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

    public void AttachState(Facility.Category category)
    {
        if (category == Facility.Category.Attack)
        {
            foreach (var crystal in crystals)
            {
                if (CrystalManager.Instance.GetCrystalAttack(crystal.type) != null)
                {

                }
            }
        }
    }
}
