using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MoneyPresenter :MonoBehaviour
{
    [SerializeField] MoneyView moneyView;

    private void Awake()
    {
        MoneyManager.OnMoneyChenged += SetMoney;
    }

    public void SetMoney(int money)
    {
        moneyView.SetMoney(money);
    }
}
