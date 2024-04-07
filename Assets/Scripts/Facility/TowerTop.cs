using UnityEngine;

public class TowerTop :MonoBehaviour
{
    Tower tower;

    private void Start()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void OnTriggerStay(Collider other)
    {
        tower.WarpToLower();
        var warpTower = Reticle.Instance.GetTower();
        if (warpTower != null)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                warpTower.WarpTowerToTower();
            }
        }
    }
}
