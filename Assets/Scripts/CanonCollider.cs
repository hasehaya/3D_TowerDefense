using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CanonCollider :MonoBehaviour
{
    [SerializeField] Canon canon;

    private void Update()
    {
        if (canon.isInstalled)
            return;
        if (canon.onTrigger)
        {
            canon.ChangeColorRed();
        }
        else
        {
            canon.ChangeColorGreen();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canon.isInstalled)
            return;
        canon.onTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (canon.isInstalled)
            return;
        canon.onTrigger = false;
    }
}
