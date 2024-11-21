using UnityEngine;

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
        NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.Descend);
    }

    public void WarpToLower()
    {
        RaycastHit hit;
        Vector3 pos = lowerPos.position + Vector3.up * 20;

        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity))
        {
            Vector3 targetPosition = hit.point + Vector3.up * 1f;
            Player.Instance.WarpTo(targetPosition);
        }
        else
        {
            Player.Instance.WarpTo(lowerPos.position);
        }
    }
}
