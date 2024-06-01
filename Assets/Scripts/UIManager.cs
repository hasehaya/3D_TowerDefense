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

    public FacilityPurchasePresenter facilityPurchasePresenter;
}
