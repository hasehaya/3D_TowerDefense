using System.Collections.Generic;

using UnityEngine;

public class FacilityInstallCollider :MonoBehaviour
{
    Facility facility;
    Collider installCol;
    Rigidbody rb;
    List<Collider> childrenCols = new List<Collider>();

    private void Start()
    {
        installCol = GetComponent<Collider>();
        facility = GetComponentInParent<Facility>();
        rb = gameObject.AddComponent<Rigidbody>();

        installCol.isTrigger = true;
        rb.mass = 0.00001f;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void Update()
    {
        if (facility.isInstalled)
            return;
        if (facility.isTouchingOtherObj)
        {
            facility.ChangeColorRed();
        }
        else
        {
            facility.ChangeColorGreen();
        }
    }

    /// <summary>
    /// ColÇ∆RbÇÕïsóvÇ…Ç»ÇÈÇÃÇ≈çÌèú
    /// </summary>
    public void InstallFacility()
    {
        Destroy(rb);
        Destroy(installCol);
    }

    public void SetChildrenCols(List<Collider> cols)
    {
        childrenCols = cols;
    }

    private void OnTriggerStay(Collider other)
    {
        if (facility.isInstalled)
            return;
        if (childrenCols.Contains(other))
            return;
        facility.isTouchingOtherObj = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (facility.isInstalled)
            return;
        if (childrenCols.Contains(other))
            return;
        facility.isTouchingOtherObj = false;
    }
}
