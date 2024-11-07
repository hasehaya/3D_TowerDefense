using UnityEngine;

public class FacilityPurchaseInputHandler :MonoBehaviour
{
    public event System.Action<int> OnNumberKeyPressed;

    private void Update()
    {
        for (int i = 0; i <= 9; i++)
        {
            KeyCode key = (KeyCode)((int)KeyCode.Alpha0 + i);
            if (Input.GetKeyDown(key))
            {
                OnNumberKeyPressed?.Invoke(i);
            }
        }
    }
}
