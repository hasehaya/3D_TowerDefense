using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SharedSceneData
{
    static int stageNum = 10;
    public static int StageNum
    {
        get { return stageNum; }
        set { stageNum = value; }
    }

    static List<Facility.Type> availableFacilityTypes = new List<Facility.Type>();
    public static List<Facility.Type> AvailableFacilityTypes
    {
        get { return availableFacilityTypes; }
        set { availableFacilityTypes = value; }
    }

    public static void ResetData()
    {
        stageNum = 0;
        availableFacilityTypes.Clear();
    }
}
