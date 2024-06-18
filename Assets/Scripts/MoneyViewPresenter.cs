using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MoneyViewPresenter :MonoBehaviour
{
    [SerializeField] MoneyView moneyView;

    private void Start()
    {
        MoneyManager.OnMoneyChenged += SetMoney;
        SetMoney(MoneyManager.Instance.Money);
    }

    public void SetMoney(int money)
    {
        moneyView.SetMoney(money);
    }
}
