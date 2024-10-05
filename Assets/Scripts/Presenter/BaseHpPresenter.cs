using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BaseHpPresenter :MonoBehaviour
{
    [SerializeField] BaseHpView baseHpView;
    void Start()
    {
        PlayerBase.OnEnemyEnterBase += UpdateHp;
    }

    void OnDestroy()
    {
        PlayerBase.OnEnemyEnterBase -= UpdateHp;
    }

    void UpdateHp(int currentHp, int maxHp)
    {
        baseHpView.UpdateHpText(currentHp, maxHp);
    }
}
