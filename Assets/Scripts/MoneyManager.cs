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

    void Start()
    {
        money = 1000;
    }

    public void Pay(int price)
    {
        money -= price;
        OnMoneyChenged?.Invoke();
    }

    public void AddMoney(int plus)
    {
        money += plus;
        OnMoneyChenged?.Invoke();
    }

    public bool CanPurchase(int price)
    {
        return money >= price;
    }

}
