using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class WaveView :MonoBehaviour
{
    [SerializeField] Text waveText;

    public void UpdateWaveText(int currentWave, int maxWave)
    {
        waveText.text = currentWave.ToString() + "/" + maxWave.ToString();
    }
}
