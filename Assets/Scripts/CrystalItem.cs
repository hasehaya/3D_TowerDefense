using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrystalItem :MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Crystal.Type type;
    [SerializeField] Image image;
    private Vector3 originalPosition;
    private Transform originalParent;

    public void SetUp(Crystal.Type type)
    {
        this.type = type;
        image.sprite = CrystalManager.Instance.GetSprite(type);
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        originalPosition = transform.position;
        originalParent = transform.parent;
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        transform.position = pointerEventData.position;
        Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        transform.position = originalPosition;
        transform.SetParent(originalParent, false);
        Debug.Log("OnEndDrag");
    }
}