using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrystalItem :MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler
{
    public Crystal crystal;

    [SerializeField] Image image;
    public Crystal.Type type;

    Transform originalParent;
    CrystalItem draggedItem;

    private void Awake()
    {
        originalParent = transform.parent;
        crystal = null;
        image.sprite = null;
        type = Crystal.Type.None;
    }

    public void SetCrystal(Crystal crystal)
    {
        if (crystal == null)
        {
            this.crystal = null;
            type = Crystal.Type.None;
            image.sprite = null;
        }
        else
        {
            this.crystal = crystal;
            type = crystal.type;
            image.sprite = crystal.sprite ?? default;
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        transform.position = pointerEventData.position;
        CrystalBox.Instance.draggedCrystal = this;
        image.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        transform.position = originalParent.position;
        CrystalBox.Instance.draggedCrystal = null;
        image.raycastTarget = true;
    }

    public void OnDrop(PointerEventData pointerEventData)
    {
        if (CrystalBox.Instance.draggedCrystal != null)
        {
            // Swap the crystals
            Crystal tempCrystal = CrystalBox.Instance.draggedCrystal.crystal;
            CrystalBox.Instance.draggedCrystal.SetCrystal(this.crystal);
            this.SetCrystal(tempCrystal);
        }
    }
}