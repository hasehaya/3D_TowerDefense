using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FacilityPurchasePresenter :MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] FacilityPurchaseView facilityPurchaseView;
    FacilityPurchaseView[] facilityPurchaseViews;

    private void Start()
    {
        var facilityInfos = FacilityManager.Instance.GetFacilityParameters();
        foreach (var facilityInfo in facilityInfos)
        {
            var facilityPurchaseView = Instantiate(this.facilityPurchaseView, content.transform);
            facilityPurchaseView.SetFacilityInfo(facilityInfo);
            facilityPurchaseView.SetButtonAction(() => OnClickPurchaseButton(facilityInfo));
        }
    }

    private void OnClickPurchaseButton(FacilityParameter facilityInfo)
    {
        var canPurchase = MoneyManager.Instance.CanPurchase(facilityInfo.price);
        if (!canPurchase)
        {
            return;
        }
        MoneyManager.Instance.Pay(facilityInfo.price);
        FacilityManager.Instance.CreateFacility(facilityInfo.type);
    }

    void ReloadPriceColor()
    {
        var facilityInfos = FacilityManager.Instance.GetFacilityParameters();
        for (int i = 0; i < facilityInfos.Length; i++)
        {
            facilityPurchaseViews[i].SetPriceColor(MoneyManager.Instance.CanPurchase(facilityInfos[i].price));
        }
    }
}
