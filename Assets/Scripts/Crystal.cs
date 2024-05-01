using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Crystal
{
    public enum Type
    {
        None = 0,
        Water = 1,
        Fire = 2,
        Wind = 3,
        Thunder = 4,
        Ice = 5,
        Wood = 6,
        Stone = 7,
        Mist = 8,
        Cloud = 9,
        ThunderCloud = 10,
        RainCloud = 11,
        SnowCloud = 12,
        ThunderStorm = 13,
        Typhoon = 14,
    }

    public Type type;
    public Sprite sprite;

    public Crystal()
    {
        type = Type.None;
        sprite = null;
    }

    public Crystal(Type type, Sprite sprite)
    {
        this.type = type;
        this.sprite = sprite;
    }
}

[CreateAssetMenu]
public class CrystalListEntity :ListEntityBase<Crystal>
{
    protected override string spreadSheetURL()
    {
        return "https://docs.google.com/spreadsheets/d/e/2PACX-1vSiKrcuetoqEFCe4BjJBB3U9V6WNiQXGiYa-vNdG1OWwfd78kXXfEVFHBDX5yKB7zQ1d1orVLzlWvIa/pub?gid=0&single=true&output=csv";
    }
}

#if UNITY_EDITOR
// CrystalListEntity のインスペクタにデータ更新ボタンを表示するクラス
[CustomEditor(typeof(CrystalListEntity))]
public class CrystalListEntityEditor :Editor
{
    public override void OnInspectorGUI()
    {
        // デフォルトのインスペクタを表示
        base.OnInspectorGUI();

        // データ更新ボタンを表示
        if (GUILayout.Button("データ更新"))
        {
            ((CrystalListEntity)target).LoadSheetData();
        }
    }
}
#endif