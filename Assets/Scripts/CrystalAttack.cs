using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class CrystalAttack
{
    public Crystal.Type type;
    public Material material;
    public float attackPower;
    public float attackSpeed;
    public float attackRate;
    public bool isAreaAttack;
    public float attackRange;
    public float attackArea;

    public CrystalAttack()
    {
        type = Crystal.Type.None;
        material = null;
        attackPower = 0;
        attackSpeed = 0;
        attackRate = 0;
        isAreaAttack = false;
        attackRange = 0;
        attackArea = 0;
    }
}

[CreateAssetMenu]
public class CrystalAttackListEntity :ListEntityBase<CrystalAttack>
{
    protected override string spreadSheetURL()
    {
        return "https://docs.google.com/spreadsheets/d/e/2PACX-1vSiKrcuetoqEFCe4BjJBB3U9V6WNiQXGiYa-vNdG1OWwfd78kXXfEVFHBDX5yKB7zQ1d1orVLzlWvIa/pub?output=csv";
    }
}

#if UNITY_EDITOR
// CrystalAttackListEntityのインスペクタにデータ更新ボタンを表示するクラス
[CustomEditor(typeof(CrystalAttackListEntity))]
public class CrystalAttackListEntityEditor :Editor
{
    public override void OnInspectorGUI()
    {
        // デフォルトのインスペクタを表示
        base.OnInspectorGUI();

        // データ更新ボタンを表示
        if (GUILayout.Button("データ更新"))
        {
            ((CrystalAttackListEntity)target).LoadSheetData();
        }
    }
}
#endif