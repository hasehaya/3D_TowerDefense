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

    public Tower GetTower()
    {
        RaycastHit hit;
        int layerToTarget = 9;
        LayerMask layerMask = 1 << layerToTarget;
        Vector3 direction = transform.position - Camera.main.transform.position;
        Physics.Raycast(Camera.main.transform.position, direction, out hit, Mathf.Infinity, layerMask);
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
        Vector3 direction = transform.position - Camera.main.transform.position;
        Physics.Raycast(Camera.main.transform.position, direction, out hit, 20, layerMask);
        if (hit.collider != null)
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public Facility GetFacility()
    {
        RaycastHit hit;
        int layerToTarget = 9;
        LayerMask layerMask = 1 << layerToTarget;
        Vector3 direction = transform.position - Camera.main.transform.position;
        Physics.Raycast(Camera.main.transform.position, direction, out hit, 30, layerMask);
        if (hit.collider != null)
        {
            return hit.collider.GetComponentInParent<Facility>();
        }
        return null;
    }
}