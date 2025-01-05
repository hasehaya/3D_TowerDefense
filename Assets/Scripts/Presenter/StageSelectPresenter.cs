using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class StageSelectPresenter :MonoBehaviour
{
    [SerializeField] GameObject stageSelectViewPrefab;
    [SerializeField] Transform stageSelectContent;
    [SerializeField] FacilitySelectPresenter facilitySelectPresenter;
    StageData[] stages;

    void Start()
    {
        var clearStagehNum = SaveDataManager.Instance.SaveData.ClearStageNum;
        stages = ScriptableObjectManager.Instance.GetStageDataArray().Where(stage => stage.stageNum <= clearStagehNum + 1).ToArray();
        foreach (var stage in stages)
        {
            var viewObj = Instantiate(stageSelectViewPrefab, stageSelectContent);
            var view = viewObj.GetComponent<StageSelectView>();
            view.AddStageButton(stage.stageNum, stage.stageName, stage.stageIcon, () => OnClickStageButton(stage));
        }
    }

    void OnClickStageButton(StageData stageData)
    {
        SharedSceneData.StageNum = stageData.stageNum;

        var availableFacilities = SaveDataManager.Instance.SaveData.GetAvailableFacilityTypeList();
        if (availableFacilities.Count <= 5)
        {
            StartCoroutine(SceneLoader.Instance.LoadScene(stageData.sceneName));
            SharedSceneData.AvailableFacilityTypes = availableFacilities;
        }
        else
        {
            facilitySelectPresenter.Initialize(stageData);
        }
    }
}
