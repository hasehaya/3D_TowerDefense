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
        //�����{�݂��Ȃ����A�{�݂��͈͊O�Ȃ�A�O�̎{�݂�OutLine���\���ɂ���
        if (targetFacility == null || !targetFacility.isInRange)
        {
            if (previousTargetFacility != null)
            {
                previousTargetFacility.isSelected = false;
                previousTargetFacility.HandleSelection(false);
                previousTargetFacility = null;
                UIManager.Instance.HideSynthesizeNotice();
            }
            return;
        }
        if (previousTargetFacility == null)
        {
            previousTargetFacility = targetFacility;
            targetFacility.isSelected = true;
            targetFacility.HandleSelection(true);
            UIManager.Instance.ShowSynthesizeNotice();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {

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
