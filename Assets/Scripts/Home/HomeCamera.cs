using DG.Tweening;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HomeCamera :MonoBehaviour
{
    private static HomeCamera instance;
    public static HomeCamera Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HomeCamera>();
            }
            return instance;
        }
    }

    Vector3 kTitlePosition { get { return new Vector3(309.8f, 5.52923f, 166.2f); } }
    Vector3 kStageSelectPosition { get { return new Vector3(336.56f, 5.52923f, 159.08f); } }
    const float kMoveDuration = 0.3f;

    public void MoveToStageSelect()
    {
        transform.DOMove(kStageSelectPosition, kMoveDuration);
    }

    public void MoveToTitle()
    {
        transform.DOMove(kTitlePosition, kMoveDuration);
    }

    public void SetToSelect()
    {
        transform.position = kStageSelectPosition;
    }
}
