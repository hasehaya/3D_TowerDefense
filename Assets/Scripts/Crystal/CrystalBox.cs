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
    CrystalFrame selectedCrystalFrame = null;
    public Crystal selectedCrystal = null;

    private void Start()
    {
        selectedCrystal = null;
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

    public void SynthesizeCrystal(Crystal crystal)
    {
        crystals.Remove(crystal);
        selectedCrystalFrame.DeleteCrystal();
        ReleaseSelectedCrystalFrame(selectedCrystalFrame);
    }

    public void SetMaxCrystals(int maxCrystals)
    {
        this.maxCrystals = maxCrystals;
    }

    public void SelectCrystalFrame(CrystalFrame frame)
    {
        if (selectedCrystalFrame != null)
        {
            selectedCrystalFrame.SetSelected(false);
        }
        selectedCrystalFrame = frame;
        selectedCrystalFrame.SetSelected(true);
        selectedCrystal = selectedCrystalFrame.GetCrystal();
    }

    public void OnClickNullCrystalFrame()
    {
        if (selectedCrystalFrame == null)
        {
            return;
        }
        selectedCrystalFrame.SetSelected(false);
        selectedCrystalFrame = null;
        selectedCrystal = null;
    }

    public void ReleaseSelectedCrystalFrame(CrystalFrame frame)
    {
        if (selectedCrystalFrame == null)
        {
            return;
        }
        selectedCrystalFrame.SetSelected(false);
        selectedCrystalFrame = null;
        selectedCrystal = null;
        frame.SetSelected(false);
    }

}
