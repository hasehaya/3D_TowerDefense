using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class SaveData
{
    private int clearStageNum;
    private bool isSeOn;
    private bool isBgmOn;

    public int ClearStageNum
    {
        get { return 11; }
        set { clearStageNum = value; }
    }

    public List<Facility.Type> AvailableFacilityTypes()
    {
        List<Facility.Type> availableFacilityTypes = new List<Facility.Type>();
        var stageData = ScriptableObjectManager.Instance.GetStageDataArray();
        foreach (var stage in stageData)
        {
            if (stage.stageNum <= ClearStageNum + 1)
            {
                availableFacilityTypes.Add(stage.mustFacility);
            }
        }
        return availableFacilityTypes;
    }

    public bool IsSeOn
    {
        get { return isSeOn; }
        set { isSeOn = value; }
    }

    public bool IsBgmOn
    {
        get { return isBgmOn; }
        set { isBgmOn = value; }
    }

    public SaveData()
    {
        clearStageNum = 0;
        isSeOn = true;
        isBgmOn = true;
    }

    public void PrintContents()
    {
        Debug.Log("ClearStageNum: " + clearStageNum);
        Debug.Log("IsSeOn: " + isSeOn);
        Debug.Log("IsBgmOn: " + isBgmOn);
    }
}
