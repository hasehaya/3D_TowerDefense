using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Unity.VisualScripting;

using UnityEngine;

public class StageSelectPresenter :MonoBehaviour
{
    [SerializeField] GameObject stageSelectViewPrefab;
    [SerializeField] Transform stageSelectContent;
    [SerializeField] FacilitySelectPresenter facilitySelectPresenter;
    StageData[] stages;

    void Start()
    {
        stages = ScriptableObjectManager.Instance.GetStageDataArray();
        foreach (var stage in stages)
        {
            var viewObj = Instantiate(stageSelectViewPrefab, stageSelectContent);
            var view = viewObj.GetComponent<StageSelectView>();
            view.AddStageButton(stage.stageNum, stage.stageName, stage.stageIcon, () => OnClickStageButton(stage));
        }
    }

    void OnClickStageButton(StageData stageData)
    {
        var availableFacilities = SaveDataManager.Instance.SaveData.AvailableFacilityTypes();
        if (availableFacilities.Count <= 5)
        {
            StartCoroutine(SceneLoader.Instance.LoadScene(stageData.sceneName));
            SharedSceneData.AvailableFacilityTypes = availableFacilities.ToListPooled();
        }
        else
        {
            facilitySelectPresenter.Initialize(stageData);
        }
    }
}
