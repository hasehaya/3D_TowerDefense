using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrystalItem :MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler
{
    public Crystal crystal;

    [SerializeField] Image image;
    public Crystal.Type type;

    Transform originalParent;

    private void Awake()
    {
        originalParent = transform.parent;
        crystal = null;
        image.sprite = null;
        type = Crystal.Type.None;
    }

    public void DeleteCrystal()
    {
        crystal = null;
        type = Crystal.Type.None;
        image.sprite = null;
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
        var draggedCrystal = CrystalBox.Instance.draggedCrystal;
        if (draggedCrystal != null)
        {
            var synthesizedCrystal = CrystalManager.Instance.GetSynthesizedCrystal(draggedCrystal.type, this.crystal.type);
            if (synthesizedCrystal == Crystal.Type.None)
            {

                Crystal tempCrystal = CrystalBox.Instance.draggedCrystal.crystal;
                draggedCrystal.SetCrystal(this.crystal);
                this.SetCrystal(tempCrystal);
            }
            else
            {
                DeleteCrystal();
                draggedCrystal.DeleteCrystal();
                var crystalItem = CrystalManager.Instance.Init(synthesizedCrystal);
                SetCrystal(crystalItem);
            }
        }
    }
}