using UnityEngine;

public class Tower :Facility
{
    [SerializeField] Transform topPos;
    [SerializeField] Transform lowerPos;

    protected override void Update()
    {
        base.Update();
        if (isSelected)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                WarpToTop();
            }
        }
    }

    public override void HandleSelection(bool isSelected)
    {
        base.HandleSelection(isSelected);
        if (isSelected)
        {
            UIManager.Instance.ShowTowerClimbNotice();
        }
        else
        {
            UIManager.Instance.HideTowerClimbNotice();
        }
    }

    public void WarpTowerToTower()
    {
        Player.Instance.transform.position = topPos.position;
    }

    public void WarpToTop()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Player.Instance.transform.position = topPos.position;
        }
    }

    public void WarpToLower()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Player.Instance.transform.position = lowerPos.position;
        }
    }
}