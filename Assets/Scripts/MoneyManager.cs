using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager :MonoBehaviour
{
    private static MoneyManager instance;
    public static MoneyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MoneyManager>();
            }
            return instance;
        }
    }
    int money;
    [SerializeField] Text moneyText;
    void Start()
    {
        money = 0;
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }

    }

    public void Pay(int price)
    {
        money -= price;
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
    }

    public void GetMoney(int plus)
    {
        money += plus;
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
    }

    public bool CanPurchase(int price)
    {
        return money >= price;
    }

}
