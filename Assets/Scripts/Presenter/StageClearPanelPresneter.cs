using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class StageClearPanelPresneter :MonoBehaviour
{
    [SerializeField] Button restartBtn;
    [SerializeField] Button homeBtn;

    private void Start()
    {
        restartBtn.onClick.AddListener(OnClickRetryButton);
        homeBtn.onClick.AddListener(OnClickHomeButton);
    }

    private void OnClickHomeButton()
    {
        StageManager.Instance.GoToHome();
    }

    private void OnClickRetryButton()
    {
        StageManager.Instance.RestartStage();
    }
}
