using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MoneyManager :MonoBehaviour
{
    // 合成された際に呼ばれるイベント
    public delegate void MoneyChenged();
    public static event MoneyChenged OnMoneyChenged;

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
    public int Money => money;
    int money;
    [SerializeField] Text moneyText;

    void Start()
    {
        money = 1000;
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
        OnMoneyChenged?.Invoke();
    }

    public void AddMoney(int plus)
    {
        money += plus;
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
        OnMoneyChenged?.Invoke();
    }

    public bool CanPurchase(int price)
    {
        return money >= price;
    }

}
