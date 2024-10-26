using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MoneyManager
{
    // 合成された際に呼ばれるイベント
    public static Action<int> OnMoneyChenged;

    private static MoneyManager instance;
    public static MoneyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MoneyManager();
            }
            return instance;
        }
    }
    private int money;
    public int Money => money;

    private MoneyManager()
    {
        AddMoney(100);
    }

    public void Pay(int price)
    {
        money -= price;
        OnMoneyChenged?.Invoke(money);
    }

    public void AddMoney(int plus)
    {
        money += plus;
        OnMoneyChenged?.Invoke(money);
    }

    public bool CanPurchase(int price)
    {
        return money >= price;
    }

}
