using System.Collections.Generic;

using UnityEngine;

public class FacilityPurchasePresenter :MonoBehaviour
{
    [SerializeField] private GameObject facilityPurchaseViewPrefab;

    private List<FacilityPurchaseItem> facilityPurchaseItems = new List<FacilityPurchaseItem>();

    private void Start()
    {
        var availableFacilityParameterList = FacilityManager.Instance.GetAvailableFacilityParameterList();

        int index = 1;
        foreach (var facilityParameter in availableFacilityParameterList)
        {
            var purchaseObj = Instantiate(facilityPurchaseViewPrefab, gameObject.transform);
            var facilityPurchaseView = purchaseObj.GetComponent<FacilityPurchaseView>();
            var facilityPurchaseModel = new FacilityPurchaseModel(index, facilityParameter);

            // Initialize the view
            facilityPurchaseView.Initialize(index, facilityPurchaseModel.Icon, facilityPurchaseModel.Price);

            // Subscribe to view events
            facilityPurchaseView.OnNumberKeyPressed += OnNumberKeyPressed;

            var item = new FacilityPurchaseItem
            {
                View = facilityPurchaseView,
                Model = facilityPurchaseModel,
            };
            facilityPurchaseItems.Add(item);

            index++;
        }

        MoneyManager.OnMoneyChanged += ReloadPriceColor;
        ReloadPriceColor(MoneyManager.Instance.Money);
    }

    private void OnNumberKeyPressed(int index)
    {
        var item = facilityPurchaseItems.Find(x => x.Model.Index == index);
        if (item != null)
        {
            AttemptPurchase(item.Model);
        }
    }

    private void AttemptPurchase(FacilityPurchaseModel model)
    {
        var canPurchase = MoneyManager.Instance.CanPurchase(model.Price);
        if (!canPurchase)
        {
            // Provide feedback to the user that purchase is not possible
            return;
        }
        var purchasingFacility = FacilityManager.Instance.GetPurchasingFacility();
        if (purchasingFacility != null)
        {
            // Handle case when a purchase is already in progress
            return;
        }
        FacilityManager.Instance.PurchaseFacility(model.Parameter.type);
    }

    private void ReloadPriceColor(int money)
    {
        foreach (var item in facilityPurchaseItems)
        {
            var price = item.Model.Price;
            var canPurchase = price <= money;
            item.View.SetPriceColor(canPurchase);
        }
    }
}


public class FacilityPurchaseItem
{
    public FacilityPurchaseView View;
    public FacilityPurchaseModel Model;
}
