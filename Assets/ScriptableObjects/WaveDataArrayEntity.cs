using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class WaveDataArrayEntity :ScriptableObject
{
    public WaveData[] array;
    string spreadSheetURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vSiKrcuetoqEFCe4BjJBB3U9V6WNiQXGiYa-vNdG1OWwfd78kXXfEVFHBDX5yKB7zQ1d1orVLzlWvIa/pub?gid=2109222210&single=true&output=csv";

#if UNITY_EDITOR
    //スプレットシートの情報をsheetDataRecordに反映させるメソッド
    public void LoadSheetData()
    {
        // urlからCSV形式の文字列をダウンロードする
        using UnityWebRequest request = UnityWebRequest.Get(spreadSheetURL);
        request.SendWebRequest();
        while (request.isDone == false)
        {
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
        }

        // ダウンロードしたCSVをデシリアライズ(SerializeFieldに入力)する
        array = CSVSerializer.Deserialize<WaveData>(request.downloadHandler.text);

        // データの更新が完了したら、ScriptableObjectを保存する
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();

        Debug.Log(" データの更新を完了しました");
    }
#endif
}

#if UNITY_EDITOR
// CrystalListEntity のインスペクタにデータ更新ボタンを表示するクラス
[CustomEditor(typeof(WaveDataArrayEntity))]
public class WaveDataListEntityEditor :Editor
{
    public override void OnInspectorGUI()
    {
        // デフォルトのインスペクタを表示
        base.OnInspectorGUI();

        // データ更新ボタンを表示
        if (GUILayout.Button("データ更新"))
        {
            ((WaveDataArrayEntity)target).LoadSheetData();
        }
    }
}
#endif