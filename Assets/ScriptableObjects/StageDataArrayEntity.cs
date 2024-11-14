using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu]
public class StageDataArrayEntity :ScriptableObject
{
    public StageData[] array;
}

[System.Serializable]
public class StageData
{
    public int stageNum;
    public SceneLoader.SceneName sceneName;
    public string stageName;
    public Sprite stageIcon;
}