using UnityEngine;

public class Canon :MonoBehaviour
{
    [SerializeField] Collider col;
    [SerializeField] Rigidbody rb;
    MeshRenderer mr;
    Color originColor;
    public bool isInstalled;
    public bool onTrigger;


    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        col.isTrigger = true;
        onTrigger = false;
        originColor = mr.material.color;
    }

    private void Update()
    {
        if (isInstalled)
            return;
        var groundPos = Reticle.Instance.GetTansform();
        if (groundPos != default)
        {
            transform.position = groundPos;
            if (onTrigger)
                return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                isInstalled = true;
                col.isTrigger = false;
                Destroy(rb);
                mr.material.color = originColor;
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
}
