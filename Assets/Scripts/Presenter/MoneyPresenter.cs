using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MoneyPresenter :MonoBehaviour
{
    [SerializeField] MoneyView moneyView;

    private void Awake()
    {
        MoneyManager.OnMoneyChanged += SetMoney;
    }

    private void OnDestroy()
    {
        MoneyManager.OnMoneyChanged -= SetMoney;
    }

    public void SetMoney(int money)
    {
        moneyView.SetMoney(money);
    }
}
