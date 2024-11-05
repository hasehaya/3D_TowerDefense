using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UpdateCaller :MonoBehaviour
{
    private static UpdateCaller instance;
    public static void AddUpdateCallback(Action updateMethod)
    {
        if (instance == null)
        {
            instance = FindObjectOfType<UpdateCaller>();
        }
        instance.updateCallback += updateMethod;
    }

    private Action updateCallback;

    private void Update()
    {
        updateCallback?.Invoke();
    }

    public static void RemoveUpdateCallback(Action updateMethod)
    {
        if (instance == null)
        {
            instance = FindObjectOfType<UpdateCaller>();
        }
        instance.updateCallback -= updateMethod;
    }
}
