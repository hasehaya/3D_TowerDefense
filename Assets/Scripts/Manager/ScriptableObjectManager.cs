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

    [SerializeField] EnemyParameterListEntity enemyParameterListEntity;
    [SerializeField] FacilityParameterListEntity facilityParameterListEntity;
    [SerializeField] StageDataListEntity stageDataListEntity;
    [SerializeField] WaveDataListEntity waveDataListEntity;
    [SerializeField] WaveEnemyDataListEntity waveEnemyDataListEntity;

    public EnemyParameterListEntity GetEnemyParameterListEntity()
    {
        return enemyParameterListEntity;
    }

    public FacilityParameterListEntity GetFacilityParameterListEntity()
    {
        return facilityParameterListEntity;
    }

    public StageDataListEntity GetStageDataListEntity()
    {
        return stageDataListEntity;
    }

    public WaveDataListEntity GetWaveDataListEntity()
    {
        return waveDataListEntity;
    }

    public WaveEnemyDataListEntity GetWaveEnemyDataListEntity()
    {
        return waveEnemyDataListEntity;
    }
}
