using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MoneyManager :MonoBehaviour
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
                instance = FindObjectOfType<MoneyManager>();
            }
            return instance;
        }
    }
    public int Money { get; private set; }

    private void Start()
    {
        Money = 300;
    }

    public void Pay(int price)
    {
        Money -= price;
        OnMoneyChenged?.Invoke(Money);
    }

    public void AddMoney(int plus)
    {
        Money += plus;
        OnMoneyChenged?.Invoke(Money);
    }

    public bool CanPurchase(int price)
    {
        return Money >= price;
    }

}
