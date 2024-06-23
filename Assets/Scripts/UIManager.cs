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
    [SerializeField] Text stageClearText;

    public void ShowStageClearText()
    {
        stageClearText.gameObject.SetActive(true);
    }
}
