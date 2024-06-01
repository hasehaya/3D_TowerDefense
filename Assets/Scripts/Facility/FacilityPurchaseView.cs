using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class FacilityPurchaseView :MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text nameText;
    [SerializeField] Text priceText;
    [SerializeField] Button parchaseButton;


    public void SetFacilityParameter(FacilityParameter facilityParameter)
    {
        SetIcon(facilityParameter.icon);
        SetName(facilityParameter.name);
        SetPrice(facilityParameter.price);
    }

    void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetPrice(int price)
    {
        priceText.text = price.ToString();
    }

    /// <summary>
    /// True：白色、False：赤色
    /// </summary>
    public void SetPriceColor(bool canPurchase)
    {
        priceText.color = canPurchase ? Color.black : Color.red;
    }

    public void SetButtonAction(System.Action action)
    {
        parchaseButton.onClick.AddListener(() => action());
    }
}
