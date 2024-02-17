using UnityEngine;

public class Tower :MonoBehaviour
{
    [SerializeField] Transform topPos;
    [SerializeField] Transform lowerPos;

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