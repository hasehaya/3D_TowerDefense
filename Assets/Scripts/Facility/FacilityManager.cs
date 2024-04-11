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

    private void Update()
    {
        var targetFacility = Reticle.Instance.GetFacility();

        if (targetFacility == null || !targetFacility.isInRange)
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
            NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.Synthesize, targetFacility.Synthesize);
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
}
