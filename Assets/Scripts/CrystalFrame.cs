using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrystalFrame :MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler
{
    Crystal crystal;
    Image crystalImage;
    [SerializeField] Image selectFrame;

    private void Awake()
    {
        crystal = null;
        crystalImage = null;
        selectFrame.enabled = false;
    }

    bool HasCrystal()
    {
        return crystal != null && crystalImage != null;
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
    /// 0の状態からセットする際に使用
    /// </summary>
    public void SetFrame(Crystal crystal, Image image)
    {
        this.crystal = crystal;
        crystalImage = image;
        crystalImage.sprite = crystal.sprite ?? default;
        crystalImage.transform.localPosition = transform.localPosition;
    }

    /// <summary>
    /// 中身を交換する際に使用
    /// </summary>
    public void SetFrame(CrystalFrame frame)
    {
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

    public void SetSelected(bool isSelected)
    {
        selectFrame.enabled = isSelected;
    }

    public void OnClickFrame()
    {
        if (HasCrystal())
        {
            CrystalBox.Instance.SelectCrystalFrame(this);
        }
        else
        {
            CrystalBox.Instance.OnClickNullCrystalFrame();
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if (!HasCrystal())
        {
            return;
        }
        //最前列に表示
        crystalImage.transform.SetAsLastSibling();
        crystalImage.transform.position = pointerEventData.position;
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (!HasCrystal())
            return;
        crystalImage.transform.localPosition = transform.localPosition;
    }

    public void OnDrop(PointerEventData pointerEventData)
    {
        var draggedFrame = pointerEventData.pointerDrag.GetComponent<CrystalFrame>();
        var draggedCrystal = draggedFrame.GetCrystal();
        //ドラッグしているCrystalがNullなら何も起こさない
        if (draggedCrystal == null)
        {
            return;
        }
        CrystalBox.Instance.ReleaseSelectedCrystalFrame(draggedFrame);
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
