using UnityEngine;

public class TowerTop :MonoBehaviour
{
    [SerializeField] Tower tower;
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
