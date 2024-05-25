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

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetPrice(string price)
    {
        priceText.text = price;
    }
}
