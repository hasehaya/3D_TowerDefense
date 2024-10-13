using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BaseHpPresenter :MonoBehaviour
{
    [SerializeField] BaseHpView baseHpView;
    void Awake()
    {
        PlayerBase.OnChangeBaseHp += UpdateHp;
    }

    void OnDestroy()
    {
        PlayerBase.OnChangeBaseHp -= UpdateHp;
    }

    void UpdateHp(int currentHp, int maxHp)
    {
        baseHpView.UpdateHpText(currentHp, maxHp);
    }
}
