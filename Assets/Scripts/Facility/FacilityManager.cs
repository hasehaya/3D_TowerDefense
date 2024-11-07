using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class FacilityManager
{
    private static FacilityManager instance;
    public static FacilityManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FacilityManager();
            }
            return instance;
        }
    }
    FacilityParameterListEntity facilityParameterListEntity;

    public List<Facility.Type> AvailableFacilityTypeList { get; private set; }
    List<Facility> installedFacilityList = new List<Facility>();
    Facility previousTargetFacility;
    GameObject purchasingFacilityObj;

    public FacilityManager()
    {
        facilityParameterListEntity = ScriptableObjectManager.Instance.GetFacilityParameterListEntity();
        UpdateCaller.AddUpdateCallback(Update);
    }

    public void SetAvailableFacilityTypes(List<Facility.Type> availableFacilityTypeList)
    {
        AvailableFacilityTypeList = availableFacilityTypeList;
    }

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

        /*
        if (CrystalBox.Instance.selectedCrystal == null)
        {
            NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.Synthesize);
        }
        else if (targetFacility.FacilityParameter.canAttachCrystal)
        {
            NoticeManager.Instance.ShowArgNotice(NoticeManager.NoticeType.Synthesize, targetFacility.Synthesize, CrystalBox.Instance.selectedCrystal);
        }
        */
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
        installedFacilityList.Add(facility);
    }

    public void RemoveFacility(Facility facility)
    {
        installedFacilityList.Remove(facility);
    }

    /// <summary>
    /// 建物を購入する関数、各FacilityクラスのParameterも自動で設定する
    /// </summary>
    public void PurchaseFacility(Facility.Type type)
    {
        GameObject facilityObj;
        facilityObj = Facility.GenerateFacility(type);
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
        facility.DestroyThisFacility();
        purchasingFacilityObj = null;
        facility.HideNotice();

        Facility.OnFacilityInstalled -= ReleasePurchasingFacility;
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

    public List<FacilityParameter> GetAvailableFacilityParameterList()
    {
        var ret = new List<FacilityParameter>();
        foreach (var parameter in facilityParameterListEntity.lists)
        {
            if (AvailableFacilityTypeList.Contains(parameter.type))
            {
                ret.Add(parameter);
            }
        }
        return ret;
    }

    public FacilityParameter[] GetFacilityParameters()
    {
        return facilityParameterListEntity.lists;
    }
}
