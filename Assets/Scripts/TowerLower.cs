using UnityEngine;

public class TowerLower :MonoBehaviour
{
    [SerializeField] Tower tower;
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        tower.WarpToTop();
    }
}