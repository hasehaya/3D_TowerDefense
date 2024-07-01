using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu]
public class StageDataListEntity :ScriptableObject
{
    public List<StageData> stages = new List<StageData>();
}

[System.Serializable]
public class StageData
{
    public int stageNum;
    public SceneLoader.SceneName sceneName;
    public string stageName;
    public Sprite stageIcon;
}