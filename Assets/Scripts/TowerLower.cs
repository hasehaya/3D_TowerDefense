using UnityEngine;

public class TowerLower :MonoBehaviour
{
    Tower tower;

    private void Start()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        tower.WarpToTop();
    }
}