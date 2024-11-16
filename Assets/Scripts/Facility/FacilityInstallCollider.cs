using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

// layerのInstallについてDefaultとRoadのみにあたるようにしている
public class FacilityInstallCollider :MonoBehaviour
{
    int groundArea;
    int roadArea;

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

        roadArea = NavMesh.GetAreaFromName("Road");
        groundArea = NavMesh.GetAreaFromName("Ground");
    }

    private void Update()
    {
        if (facility.isInstalled)
        {
            DestroyInstallCollider();
            return;
        }

        bool canInstall = false;

        switch (facility.GetInstallType())
        {
            case Facility.InstallType.Side:
            canInstall = IsNavMeshAreaBelow(groundArea);
            break;

            case Facility.InstallType.Road:
            canInstall = IsNavMeshAreaBelow(roadArea);
            break;
        }

        facility.canInstall = canInstall;
        print(canInstall);

        if (facility.canInstall)
        {
            facility.ChangeColorGreen();
        }
        else
        {
            facility.ChangeColorRed();
        }
    }

    /// <summary>
    /// 指定されたNavMeshエリアが現在の位置の下に存在するかを判定します。
    /// </summary>
    /// <param name="area">判定するNavMeshエリアのインデックス</param>
    /// <returns>指定されたエリアが存在する場合はtrue、それ以外はfalse</returns>
    bool IsNavMeshAreaBelow(int area)
    {
        RaycastHit hit;
        int groundLayerMask = LayerMask.GetMask("Ground");

        var upperPos = transform.position + Vector3.up * 10;
        if (Physics.Raycast(upperPos, Vector3.down, out hit, 20, groundLayerMask))
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 1f, NavMesh.AllAreas))
            {
                return (navHit.mask & (1 << area)) != 0;
            }
        }

        return false;
    }

    /// <summary>
    /// 設置後RigidBodyとColliderを削除
    /// </summary>
    public void DestroyInstallCollider()
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
