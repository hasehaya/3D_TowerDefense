using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class StageClearPanelPresneter :MonoBehaviour
{
    [SerializeField] Button restartBtn;
    [SerializeField] Button nextStageBtn;

    private void Start()
    {
        restartBtn.onClick.AddListener(OnClickRetryButton);
        nextStageBtn.onClick.AddListener(OnClickNextStageButton);
    }

    private void OnClickNextStageButton()
    {
        StageManager.Instance.NextStage();
    }

    private void OnClickRetryButton()
    {
        StageManager.Instance.RestartStage();
    }
}
