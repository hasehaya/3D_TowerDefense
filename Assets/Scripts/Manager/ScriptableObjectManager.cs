using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ScriptableObjectManager :MonoBehaviour
{
    private static ScriptableObjectManager instance;
    public static ScriptableObjectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScriptableObjectManager>();
            }
            return instance;
        }
    }

    [SerializeField] EnemyParameterArrayEntity enemyParameterArrayEntity;
    [SerializeField] FacilityParameterArrayEntity facilityParameterArrayEntity;
    [SerializeField] StageDataArrayEntity stageDataArrayEntity;
    [SerializeField] WaveDataArrayEntity waveDataArrayEntity;
    [SerializeField] WaveEnemyDataArrayEntity waveEnemyDataArrayEntity;
    [SerializeField] MessageDataArrayEntity messageDataArrayEntity;
    [SerializeField] SoundDataArrayEntity seDataArrayEntity;
    [SerializeField] SoundDataArrayEntity bgmDataArrayEntity;

    public EnemyParameter[] GetEnemyParameterArray() => enemyParameterArrayEntity.array;
    public FacilityParameter[] GetFacilityParameterArray() => facilityParameterArrayEntity.array;
    public StageData[] GetStageDataArray() => stageDataArrayEntity.array;
    public WaveData[] GetWaveDataArray() => waveDataArrayEntity.array;
    public WaveEnemyData[] GetWaveEnemyDataArray() => waveEnemyDataArrayEntity.array;
    public MessageData[] GetMessageDataArray() => messageDataArrayEntity.array;
    public Sound[] GetSoundDataArray() => seDataArrayEntity.array;
    public Sound[] GetBGMDataArray() => bgmDataArrayEntity.array;
}
