using Cinemachine;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class VirtualCameraController :MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        StageManager.OnPause += Pause;
        StageManager.OnResume += Resume;
    }

    private void OnDestroy()
    {
        StageManager.OnPause -= Pause;
        StageManager.OnResume -= Resume;
    }

    private void Pause()
    {
        virtualCamera.enabled = false;
    }

    private void Resume()
    {
        virtualCamera.enabled = true;
    }
}
