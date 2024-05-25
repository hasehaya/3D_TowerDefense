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
        var facilityInfos = FacilityManager.Instance.GetFacilityInfos();
        foreach (var facilityInfo in facilityInfos)
        {
            var facilityPurchaseView = Instantiate(this.facilityPurchaseView, content.transform);
            facilityPurchaseView.SetFacilityInfo(facilityInfo);
            facilityPurchaseView.SetButtonAction(() => OnClickPurchaseButton(facilityInfo));
        }
    }

    private void OnClickPurchaseButton(FacilityInfo facilityInfo)
    {

    }

    void ReloadPriceColor()
    {
        var facilityInfos = FacilityManager.Instance.GetFacilityInfos();
        for (int i = 0; i < facilityInfos.Length; i++)
        {
            facilityPurchaseViews[i].SetPriceColor(FacilityManager.Instance.CanPurchase(facilityInfos[i]));
        }
    }
}
