using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Facility :MonoBehaviour
{
    public bool isInstalled = true;
    public bool isTouchingOtherObj = true;
    public bool isInRange = false;

    [SerializeField] MeshRenderer mr;

    Color originColor;
    FacilityInstallCollider faciltyInstallCol;
    Outline outline;
    List<Collider> childrenCols = new List<Collider>();


    private void Start()
    {
        originColor = mr.material.color;
        faciltyInstallCol = GetComponentInChildren<FacilityInstallCollider>();

        outline = GetComponentInChildren<Outline>();
        outline.enabled = false;

        childrenCols = GetComponentsInChildren<Collider>().ToList();
        faciltyInstallCol.SetChildrenCols(childrenCols);
    }

    private void Update()
    {
        if (isInstalled)
            return;
        var groundPos = Reticle.Instance.GetTansform();
        if (groundPos != default)
        {
            transform.position = groundPos;
            if (isTouchingOtherObj)
                return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                isInstalled = true;
                mr.material.color = originColor;
                faciltyInstallCol.InstallFacility();
            }
        }
    }

    public void ChangeColorRed()
    {
        mr.material.color = new Color(1.0f, originColor.g / 3, originColor.b / 3, 0.9f);
    }

    public void ChangeColorGreen()
    {
        mr.material.color = new Color(originColor.r / 3, 1.0f, originColor.b / 3, 0.9f);
    }

    public void SetActiveOutLine(bool b)
    {
        outline.enabled = b;
    }
}
