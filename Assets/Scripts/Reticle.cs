using UnityEngine;

public class Reticle :MonoBehaviour
{
    private static Reticle instance;
    public static Reticle Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Reticle>();
            }
            return instance;
        }
    }

    private Vector3 GetCameraDirectionVector()
    {
        return transform.position - Camera.main.transform.position;
    }

    public Tower GetTower()
    {
        RaycastHit hit;
        int layerToTarget = 9;
        LayerMask layerMask = 1 << layerToTarget;
        Physics.Raycast(Camera.main.transform.position, GetCameraDirectionVector(), out hit, Mathf.Infinity, layerMask);
        if (hit.collider != null)
        {
            var tower = hit.collider.GetComponentInParent<Tower>();
            if (tower != null)
            {
                return tower;
            }
        }
        return null;
    }

    public Vector3 GetTansform()
    {
        RaycastHit hit;
        int layerToTarget = 8;
        LayerMask layerMask = 1 << layerToTarget;
        Physics.Raycast(Camera.main.transform.position, GetCameraDirectionVector(), out hit, 20, layerMask);
        if (hit.collider != null)
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// Facilityを取得、距離は20
    /// </summary>
    public Facility GetFacility()
    {
        RaycastHit hit;
        int layerToTarget = 9;
        LayerMask layerMask = 1 << layerToTarget;
        Physics.Raycast(Camera.main.transform.position, GetCameraDirectionVector(), out hit, 20, layerMask);
        if (hit.collider != null)
        {
            return hit.collider.GetComponentInParent<Facility>();
        }
        return null;
    }
}