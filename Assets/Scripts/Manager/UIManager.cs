using UnityEngine;
using UnityEngine.UI;

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

    public FacilityPurchasePresenter facilityPurchasePresenter;
    [SerializeField] StageClearPanelPresneter stageClearPanelPresneter;
    [SerializeField] GameOverPanelPresenter gameOverPanelPresenter;

    public void ShowStageClearPanel()
    {
        stageClearPanelPresneter.gameObject.SetActive(true);
    }

    public void ShowGameOverPanel()
    {
        gameOverPanelPresenter.gameObject.SetActive(true);
    }
}
