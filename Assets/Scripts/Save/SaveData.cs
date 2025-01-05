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
        get { return clearStageNum; }
        set { clearStageNum = value; }
    }

    public List<Facility.Type> GetAvailableFacilityTypeList()
    {
        List<Facility.Type> availableFacilityTypes = new List<Facility.Type>();
        var stageData = ScriptableObjectManager.Instance.GetStageDataArray();
        foreach (var stage in stageData)
        {
            if (stage.stageNum > ClearStageNum + 1)
            {
                break;
            }
            if (stage.mustFacilityType == Facility.Type.None)
            {
                continue;
            }
            availableFacilityTypes.Add(stage.mustFacilityType);
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
