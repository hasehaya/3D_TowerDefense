using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

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