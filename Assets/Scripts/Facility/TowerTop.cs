﻿using UnityEngine;

public class TowerTop :MonoBehaviour
{
    Tower tower;
    Tower currentTower = null;
    bool isShowNotice = false;

    private void Start()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isShowNotice)
        {
            isShowNotice = true;
            NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.Descend, tower.WarpToLower);
        }
        var warpTower = Reticle.Instance.GetTower();
        if (warpTower != null)
        {
            // 一度だけ通るようにする
            if (warpTower == currentTower)
                return;
            currentTower = warpTower;
            warpTower.WarpSelection(true);
        }
        else
        {
            //ワープ対象がいない場合前の対象の輪郭をなくしNullにする
            currentTower?.WarpSelection(false);
            currentTower = null;
            NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.Warp);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isShowNotice = false;
        NoticeManager.Instance.HideAllNotice();
    }
}