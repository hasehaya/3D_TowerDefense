﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class MoneyManager
{
    // 合成された際に呼ばれるイベント
    public static Action<int> OnMoneyChanged;

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
    }

    public void Pay(int price)
    {
        money -= price;
        OnMoneyChanged?.Invoke(money);
    }

    public void AddMoney(int plus)
    {
        money += plus;
        OnMoneyChanged?.Invoke(money);
    }

    public bool CanPurchase(int price)
    {
        return money >= price;
    }

}
