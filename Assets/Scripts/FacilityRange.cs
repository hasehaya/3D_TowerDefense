using UnityEngine;

public class FacilityRange :MonoBehaviour
{
    Facility facility;
    Collider rangeCol;

    private void Start()
    {
        facility = GetComponentInParent<Facility>();
        rangeCol = GetComponent<Collider>();
        rangeCol.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        facility.isInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        facility.isInRange = false;
    }
}
