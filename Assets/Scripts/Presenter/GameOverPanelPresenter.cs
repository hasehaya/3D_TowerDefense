using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelPresenter :MonoBehaviour
{
    [SerializeField] Button restartBtn;

    private void Start()
    {
        restartBtn.onClick.AddListener(OnClickRestartButton);
    }
    private void OnClickRestartButton()
    {
        StageManager.Instance.RestartStage();
    }
}
