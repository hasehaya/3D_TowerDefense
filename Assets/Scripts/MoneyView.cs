using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MoneyView :MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text moneyText;

    public void SetMoney(int money)
    {
        moneyText.text = money.ToString();
    }
}
