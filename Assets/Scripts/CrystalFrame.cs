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
    /// 一から生成するときに使用
    /// </summary>
    /// <param name="crystal"></param>
    /// <param name="image"></param>
    public void SetFrame(Crystal crystal, Image image)
    {
        this.crystal = crystal;
        crystalImage = image;
        crystalImage.sprite = crystal.sprite ?? default;
    }

    /// <summary>
    /// 入れ替えるときに使用
    /// </summary>
    /// <param name="frame"></param>
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
            crystalImage.transform.position = transform.position;
        }
    }

    public void DeleteCrystal()
    {
        crystal = null;
        Destroy(crystalImage.gameObject);
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
        crystalImage.transform.position = transform.position;
    }

    public void OnDrop(PointerEventData pointerEventData)
    {
        var draggedFrame = pointerEventData.pointerDrag.GetComponent<CrystalFrame>();
        var draggedCrystal = draggedFrame.GetCrystal();
        var synthesizedCrystal = CrystalManager.Instance.GetSynthesizedCrystal(draggedCrystal.type, crystal.type);
        if (synthesizedCrystal == Crystal.Type.None)
        {
            CrystalFrame tempCrystal = draggedFrame;
            draggedFrame.SetFrame(this);
            SetFrame(tempCrystal);
        }
        else
        {
            draggedFrame.DeleteCrystal();
            var crystalItem = CrystalManager.Instance.Init(synthesizedCrystal);
            SetFrame(crystalItem, crystalImage);
        }
    }
}
