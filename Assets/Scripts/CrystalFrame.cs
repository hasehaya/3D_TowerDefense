using UnityEngine;
using UnityEngine.EventSystems;

public class CrystalFrame :MonoBehaviour, IDropHandler
{
    CrystalItem crystalItem;
    public void OnDrop(PointerEventData pointerEventData)
    {
        CrystalItem otherCrystalItem = pointerEventData.pointerDrag.GetComponentInParent<CrystalItem>();
        if (otherCrystalItem != null)
        {
            Debug.Log("Dropped " + crystalItem.type);
            otherCrystalItem.transform.SetParent(transform, false);
        }
    }
}