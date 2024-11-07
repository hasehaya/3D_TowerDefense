using UnityEngine;
using UnityEngine.UI;

public class FacilityPurchaseView :MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text indexText;
    [SerializeField] private MoneyView moneyView;

    public event System.Action<int> OnNumberKeyPressed;

    private int facilityIndex;

    public void Initialize(int index, Sprite sprite, int price)
    {
        facilityIndex = index;
        SetIndex(index);
        SetIcon(sprite);
        SetPrice(price);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0 + facilityIndex))
        {
            OnNumberKeyPressed?.Invoke(facilityIndex);
        }
    }

    private void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    private void SetIndex(int index)
    {
        indexText.text = index.ToString();
    }

    private void SetPrice(int price)
    {
        moneyView.SetMoney(price);
    }

    public void SetPriceColor(bool canPurchase)
    {
        moneyView.SetPriceColor(canPurchase);
    }
}
