using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class FacilityCellView :MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Button button;

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetButtonAction(System.Action action)
    {
        button.onClick.AddListener(() => action());
    }

    public void SetInteractable(bool isInteractable)
    {
        button.interactable = isInteractable;
    }

}
