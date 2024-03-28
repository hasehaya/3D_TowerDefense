using UnityEngine;

public class UIManager :MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [SerializeField] GameObject synthesizeNotice;
    [SerializeField] GameObject towerClimbNotice;
    [SerializeField] GameObject towerDescendNotice;
    [SerializeField] GameObject crystalBox;


    private void Start()
    {
        HideSynthesizeNotice();
        HideTowerClimbNotice();
        HideTowerDescendNotice();
    }

    public void ShowSynthesizeNotice()
    {
        synthesizeNotice.SetActive(true);
    }

    public void HideSynthesizeNotice()
    {
        synthesizeNotice.SetActive(false);
    }

    public void ShowCrystalBox()
    {
        crystalBox.SetActive(true);
    }

    public void HideCrystalBox()
    {
        crystalBox.SetActive(false);
    }

    public void ShowTowerClimbNotice()
    {
        towerClimbNotice.SetActive(true);
    }

    public void HideTowerClimbNotice()
    {
        towerClimbNotice.SetActive(false);
    }

    public void ShowTowerDescendNotice()
    {
        towerDescendNotice.SetActive(true);
    }

    public void HideTowerDescendNotice()
    {
        towerDescendNotice.SetActive(false);
    }
}
