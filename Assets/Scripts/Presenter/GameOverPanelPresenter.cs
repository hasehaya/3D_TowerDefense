using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameOverPanelPresenter :MonoBehaviour
{
    public void OnClickRestartButton()
    {
        StageManager.Instance.RestartStage();
    }
}
