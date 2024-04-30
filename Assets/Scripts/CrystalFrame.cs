using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrystalFrame :MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler
{
    Crystal crystal;
    Image selectFrame;
    Image crystalImage;

    private void Awake()
    {
        crystal = null;
        selectFrame = GetComponent<Image>();
        crystalImage = null;
    }

    public Crystal GetCrystal()
    {
        return crystal;
    }

    public Image GetImage()
    {
        return crystalImage;
    }

    /// <summary>
    /// ???????N???X?^?????????????????g?p
    /// </summary>
    /// <param name="crystal"></param>
    /// <param name="image"></param>
    public void SetFrame(Crystal crystal, Image image)
    {
        this.crystal = crystal;
        crystalImage = image;
        crystalImage.sprite = crystal.sprite ?? default;
        crystalImage.transform.localPosition = transform.localPosition;
    }

    /// <summary>
    /// ?h???b?O???h???b?v???????g?p
    /// </summary>
    /// <param name="frame"></param>
    public void SetFrame(CrystalFrame frame)
    {
        //????????
        bool isNull = frame.GetCrystal() == null || frame.GetImage() == null;
        if (isNull)
        {
            crystal = null;
            crystalImage = null;
        }
        else
        {
            crystal = frame.GetCrystal();
            crystalImage = frame.GetImage();
            crystalImage.transform.localPosition = transform.localPosition;
        }
    }

    public void RemoveCrystal()
    {
        crystal = null;
        crystalImage = null;
    }

    public void DeleteCrystal()
    {
        Destroy(crystalImage.gameObject);
        crystal = null;
        crystalImage = null;
    }

    public void EnableSelect()
    {
        selectFrame.enabled = true;
    }

    public void DisableSelect()
    {
        selectFrame.enabled = false;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        //最前列に表示
        crystalImage.transform.SetAsLastSibling();
        crystalImage.transform.position = pointerEventData.position;
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (crystalImage == null)
            return;
        crystalImage.transform.localPosition = transform.localPosition;
    }

    public void OnDrop(PointerEventData pointerEventData)
    {
        var draggedFrame = pointerEventData.pointerDrag.GetComponent<CrystalFrame>();
        var draggedCrystal = draggedFrame.GetCrystal();
        //ドラッグしているCrystalがNullなら
        if (draggedCrystal == null)
        {
            return;
        }
        //ドロップしたCrystalがNullなら
        if (crystal == null)
        {
            SetFrame(draggedFrame);
            draggedFrame.RemoveCrystal();
            return;
        }
        var synthesizedCrystal = CrystalManager.Instance.GetSynthesizedCrystal(draggedCrystal.type, crystal.type);
        if (synthesizedCrystal == Crystal.Type.None)
        {
            CrystalFrame tempFrame = draggedFrame;
            draggedFrame.SetFrame(this);
            SetFrame(tempFrame);
        }
        else
        {
            draggedFrame.DeleteCrystal();
            var crystalItem = CrystalManager.Instance.Init(synthesizedCrystal);
            SetFrame(crystalItem, crystalImage);
        }
    }
}
