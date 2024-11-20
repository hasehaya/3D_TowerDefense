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
        Vector3 origin = lowerPos.position;
        int layerToTarget = LayerMask.NameToLayer("Ground");
        LayerMask layerMask = 1 << layerToTarget;

        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 targetPosition = hit.point + Vector3.up * 1f;
            Player.Instance.WarpTo(targetPosition);
        }
        else if (Physics.Raycast(origin, Vector3.up, out hit, Mathf.Infinity, layerMask))
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
