﻿using DG.Tweening;

using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class FacilityPurchasePresenter :MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject facilityPurchaseViewPrefab;
    List<FacilityPurchaseView> facilityPurchaseViews = new List<FacilityPurchaseView>();

    private void Start()
    {
        NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.OpenFacilityPurchase);

        var facilityParameters = FacilityManager.Instance.GetFacilityParameters();
        foreach (var facilityParameter in facilityParameters)
        {
            var purchaseObj = Instantiate(facilityPurchaseViewPrefab, content.transform);
            var facilityPurchaseView = purchaseObj.GetComponent<FacilityPurchaseView>();
            facilityPurchaseView.SetFacilityParameter(facilityParameter);
            facilityPurchaseView.SetButtonAction(() => OnClickPurchaseButton(facilityParameter));
            facilityPurchaseViews.Add(facilityPurchaseView);
        }
        MoneyManager.OnMoneyChenged += ReloadPriceColor;
        ReloadPriceColor(MoneyManager.Instance.Money);
    }

    private void OnClickPurchaseButton(FacilityParameter facilityParameter)
    {
        var canPurchase = MoneyManager.Instance.CanPurchase(facilityParameter.price);
        if (!canPurchase)
        {
            return;
        }
        var purchasingFacility = FacilityManager.Instance.GetPurchasingFacility();
        if (purchasingFacility != null)
        {
            return;
        }
        FacilityManager.Instance.PurchaseFacility(facilityParameter.type);
    }

    void ReloadPriceColor(int money)
    {
        var facilityParameters = FacilityManager.Instance.GetFacilityParameters();
        for (int i = 0; i < facilityParameters.Length; i++)
        {
            var price = facilityParameters[i].price;
            var canPurchase = price <= money;
            facilityPurchaseViews[i].SetPriceColor(canPurchase);
        }
    }

    public void OnClickCloseBtn()
    {
        transform.DOMoveX(680, 1);
        Cursor.lockState = CursorLockMode.Locked;
        NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.OpenFacilityPurchase);
    }

    public void OpenFacilityPurchase()
    {
        transform.DOMoveX(0, 1);
        Cursor.lockState = CursorLockMode.None;
    }
}
