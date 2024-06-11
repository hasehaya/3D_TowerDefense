using System.Collections.Generic;

using UnityEngine;

// layerのInstallについてDefaultとRoadのみにあたるようにしている
public class FacilityInstallCollider :MonoBehaviour
{
    Facility facility;
    Collider installCol;
    Rigidbody rb;
    List<Collider> childrenCols = new List<Collider>();
    List<Collider> touchingObjs = new List<Collider>();

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
        switch (facility.GetInstallType())
        {
            case Facility.InstallType.Side:
            if (touchingObjs.Count == 0)
            {
                facility.canInstall = true;
            }
            else
            {
                facility.canInstall = false;
            }
            break;
            case Facility.InstallType.Road:
            if (IsTouchingObj("Road"))
            {
                facility.canInstall = true;
            }
            else
            {
                facility.canInstall = false;
            }
            break;
        }
        if (facility.canInstall)
        {
            facility.ChangeColorGreen();
        }
        else
        {
            facility.ChangeColorRed();
        }
    }

    bool IsTouchingObj(string tag)
    {
        foreach (var touchingObj in touchingObjs)
        {
            if (touchingObj.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 設置後RigidBodyとColliderを削除
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
        if (touchingObjs.Contains(other))
            return;
        touchingObjs.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (facility.isInstalled)
            return;
        if (childrenCols.Contains(other))
            return;
        if (!touchingObjs.Contains(other))
            return;
        touchingObjs.Remove(other);
    }
}
