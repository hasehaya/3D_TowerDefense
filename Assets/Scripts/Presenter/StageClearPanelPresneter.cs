using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class StageClearPanelPresneter :MonoBehaviour
{
    public void OnClickNextStageButton()
    {
        StageManager.Instance.NextStage();
    }

    public void OnClickRetryButton()
    {
        StageManager.Instance.RestartStage();
    }
}
