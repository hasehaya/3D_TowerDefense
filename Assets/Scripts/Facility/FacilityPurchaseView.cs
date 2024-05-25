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


    public void SetFacilityInfo(FacilityInfo facilityInfo)
    {
        SetIcon(facilityInfo.icon);
        SetName(facilityInfo.name);
        SetPrice(facilityInfo.price);
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetName(string name)
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
        priceText.color = canPurchase ? Color.white : Color.red;
    }

    public void SetButtonAction(System.Action action)
    {
        parchaseButton.onClick.AddListener(() => action());
    }
}
