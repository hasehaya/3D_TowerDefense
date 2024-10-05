using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class WavePresenter :MonoBehaviour
{
    [SerializeField] WaveView waveView;

    private void Start()
    {
        WaveManager.OnWaveChanged += UpdateWaveText;
    }

    private void OnDestroy()
    {
        WaveManager.OnWaveChanged -= UpdateWaveText;
    }

    void UpdateWaveText(int currentWave, int maxWave)
    {
        waveView.UpdateWaveText(currentWave, maxWave);
    }
}
