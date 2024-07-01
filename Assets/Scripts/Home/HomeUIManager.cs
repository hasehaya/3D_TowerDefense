using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HomeUIManager :MonoBehaviour
{
    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject stageSelectUI;


    public void OnClickGameStartBtn()
    {
        HomeCamera.Instance.MoveToStageSelect();
        titleUI.SetActive(false);
        stageSelectUI.SetActive(true);
    }

    public void OnClickTitleBtn()
    {
        HomeCamera.Instance.MoveToTitle();
        titleUI.SetActive(true);
        stageSelectUI.SetActive(false);
    }
}
