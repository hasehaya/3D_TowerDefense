using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu]
public class CrystalCombinationListEntity :ScriptableObject
{
    public List<CrystalCombination> crystalCombinations = new List<CrystalCombination>();
}

[System.Serializable]
public class CrystalCombination
{
    public Crystal.Type[] synthesisSourceCrystals;
    public Crystal.Type synthesizedCrystal;
}
