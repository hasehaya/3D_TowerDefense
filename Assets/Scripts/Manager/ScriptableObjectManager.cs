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

    [SerializeField] EnemyParameterArrayEntity enemyParameterListEntity;
    [SerializeField] FacilityParameterArrayEntity facilityParameterListEntity;
    [SerializeField] StageDataArrayEntity stageDataListEntity;
    [SerializeField] WaveDataArrayEntity waveDataListEntity;
    [SerializeField] WaveEnemyDataArrayEntity waveEnemyDataListEntity;
    [SerializeField] MessageDataArrayEntity messageDataListEntity;
    [SerializeField] SoundDataArrayEntity seDataListEntity;
    [SerializeField] SoundDataArrayEntity bgmDataListEntity;

    public EnemyParameter[] GetEnemyParameterArray() => enemyParameterListEntity.array;
    public FacilityParameter[] GetFacilityParameterArray() => facilityParameterListEntity.array;
    public StageData[] GetStageDataArray() => stageDataListEntity.array;
    public WaveData[] GetWaveDataArray() => waveDataListEntity.array;
    public WaveEnemyData[] GetWaveEnemyDataArray() => waveEnemyDataListEntity.array;
    public MessageData[] GetMessageDataArray() => messageDataListEntity.array;
    public Sound[] GetSoundDataArray() => seDataListEntity.array;
    public Sound[] GetBGMDataArray() => bgmDataListEntity.array;
}
