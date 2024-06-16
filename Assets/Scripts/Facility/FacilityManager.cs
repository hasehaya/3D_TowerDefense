using System.Collections.Generic;

using UnityEngine;

public class FacilityManager :MonoBehaviour
{
    private static FacilityManager instance;
    public static FacilityManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FacilityManager>();
            }
            return instance;
        }
    }

    [SerializeField] GameObject facilityPrefab;
    [SerializeField] FacilityAttackParameterListEntity attackParameterListEntity;
    [SerializeField] FacilityParameterListEntity facilityParameterListEntity;

    List<Facility> facilities = new List<Facility>();
    Facility previousTargetFacility;
    GameObject purchasingFacilityObj;

    private void Update()
    {
        var targetFacility = Reticle.Instance.GetFacility();

        if (targetFacility == null)
        {
            if (previousTargetFacility != null)
            {
                previousTargetFacility.isSelected = false;
                previousTargetFacility.HandleSelection(false);
                previousTargetFacility = null;
            }
            return;
        }

        if (previousTargetFacility == null)
        {
            previousTargetFacility = targetFacility;
            targetFacility.isSelected = true;
            targetFacility.HandleSelection(true);
        }

        if (CrystalBox.Instance.selectedCrystal == null)
        {
            NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.Synthesize);
        }
        else if (targetFacility.FacilityParameter.canAttachCrystal)
        {
            NoticeManager.Instance.ShowArgNotice(NoticeManager.NoticeType.Synthesize, targetFacility.Synthesize, CrystalBox.Instance.selectedCrystal);
        }
    }

    public GameObject GetPurchasingFacility()
    {
        return purchasingFacilityObj;
    }

    void ReleasePurchasingFacility(Facility facility)
    {
        Facility.OnFacilityInstalled -= ReleasePurchasingFacility;
        var purchasingFacility = purchasingFacilityObj.GetComponent<Facility>();
        if (purchasingFacility == facility)
        {
            purchasingFacilityObj = null;
        }
    }

    public void AddFacility(Facility facility)
    {
        facilities.Add(facility);
    }

    public void RemoveFacility(Facility facility)
    {
        facilities.Remove(facility);
    }

    /// <summary>
    /// 建物を購入する関数、各FacilityクラスのParameterも自動で設定する
    /// </summary>
    public void PurchaseFacility(Facility.Type type)
    {
        GameObject facilityObj;
        if (IsFacilityAttackExist(type))
        {
            facilityObj = FacilityAttack.GenerateFacilityAttack(type);
        }
        else
        {
            facilityObj = Facility.GenerateFacility(type);
        }
        purchasingFacilityObj = facilityObj;
        var facility = facilityObj.GetComponent<Facility>();
        MoneyManager.Instance.Pay(facility.FacilityParameter.price);
        AddFacility(facility);
        NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.PurchaseCancel, PurchaseCancel);

        Facility.OnFacilityInstalled += ReleasePurchasingFacility;
    }

    public void PurchaseCancel()
    {
        var facility = purchasingFacilityObj.GetComponent<Facility>();
        MoneyManager.Instance.AddMoney(facility.FacilityParameter.price);
        RemoveFacility(facility);
        Destroy(purchasingFacilityObj);
        purchasingFacilityObj = null;
        NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.OpenFacilityPurchase);
        facility.HideNotice();

        Facility.OnFacilityInstalled -= ReleasePurchasingFacility;
    }

    public bool IsFacilityAttackExist(Facility.Type type)
    {
        foreach (var facility in attackParameterListEntity.lists)
        {
            if (facility.type == type)
            {
                return true;
            }
        }
        return false;
    }

    public FacilityParameter GetFacilityParameter(Facility.Type type)
    {
        foreach (var parameter in facilityParameterListEntity.lists)
        {
            if (parameter.type == type)
            {
                return parameter;
            }
        }
        return null;
    }

    public FacilityAttackParameter GetFacilityAttackParameter(Facility.Type type)
    {
        foreach (var parameter in attackParameterListEntity.lists)
        {
            if (parameter.type == type)
            {
                return parameter;
            }
        }
        return null;
    }

    public FacilityParameter[] GetFacilityParameters()
    {
        return facilityParameterListEntity.lists;
    }
}
