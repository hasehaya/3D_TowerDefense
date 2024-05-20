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

    List<Facility> facilities = new List<Facility>();
    Facility previousTargetFacility;
    [SerializeField] GameObject facilityPrefab;
    GameObject purchaseFacility;

    private void Start()
    {
        NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.Purchase, CreateFacility);
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

        if (CrystalBox.Instance.selectedCrystal == null)
        {
            NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.Synthesize);
        }
        else
        {
            NoticeManager.Instance.ShowArgNotice(NoticeManager.NoticeType.Synthesize, targetFacility.Synthesize, CrystalBox.Instance.selectedCrystal);
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

    public void CreateFacility()
    {
        GameObject createFacility = Instantiate(facilityPrefab);
        purchaseFacility = createFacility;
        var facility = purchaseFacility.GetComponent<Facility>();
        NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.PurchaseCancel, PurchaseCancel);
        AddFacility(facility);
    }

    public void PurchaseCancel()
    {
        Destroy(purchaseFacility);
        var facility = purchaseFacility.GetComponent<Facility>();
        RemoveFacility(facility);
        NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.Purchase, CreateFacility);
    }
}
