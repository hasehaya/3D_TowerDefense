using System.Collections;
using System.Collections.Generic;

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
            view.AddStageButton(stage.stageNum, stage.stageName, stage.stageIcon, () => OnClickStageButton(stage.sceneName));
        }
    }

    void OnClickStageButton(SceneLoader.SceneName sceneName)
    {
        facilitySelectPresenter.Initialize(sceneName);
    }
}
