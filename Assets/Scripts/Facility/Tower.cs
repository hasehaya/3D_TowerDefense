﻿using UnityEngine;

public class Tower :Facility
{
    [SerializeField] Transform topPos;
    [SerializeField] Transform lowerPos;

    protected override void AddNoticeTypes()
    {
        base.AddNoticeTypes();
        noticeTypes.Add(NoticeManager.NoticeType.Climb);
        noticeTypes.Add(NoticeManager.NoticeType.Descend);
    }

    public override void HandleSelection(bool isSelected)
    {
        base.HandleSelection(isSelected);
        if (isSelected)
        {
            NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.Climb, WarpToTop);
        }
    }

    /// <summary>
    /// ワープの対象を選んだ時輪郭を黄色にするよう
    /// </summary>
    /// <param name="isSelected"></param>
    public void WarpSelection(bool isSelected)
    {
        outline.enabled = isSelected;
        if (isSelected)
        {
            NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.Warp, WarpToTop);
        }
        else
        {
            NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.Warp);
        }
    }

    public void WarpToTop()
    {
        Player.Instance.WarpTo(topPos.position);
    }

    public void WarpToLower()
    {
        Player.Instance.WarpTo(lowerPos.position);
    }
}