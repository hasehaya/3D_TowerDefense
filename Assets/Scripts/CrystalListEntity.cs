using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "CrystalListEntity", menuName = "CrystalListEntity")]
public class CrystalListEntity :ScriptableObject
{
    public List<Crystal> crystals = new List<Crystal>();
}
