﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class StageSelectView :MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Text stageNameText;
    [SerializeField] Image stageIconImage;
    [SerializeField] Image selectedFrame;
    int stageNum;

    public void AddStageButton(int num, string name, Sprite icon, UnityEngine.Events.UnityAction action)
    {
        stageNum = num;
        stageNameText.text = name;
        stageIconImage.sprite = icon;
        button.onClick.AddListener(action);
    }

    public void SetSelected(bool isSelected)
    {
        selectedFrame.enabled = isSelected;
    }
}
