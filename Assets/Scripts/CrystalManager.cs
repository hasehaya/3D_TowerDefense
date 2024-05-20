using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class CrystalManager :MonoBehaviour
{
    private static CrystalManager instance;
    public static CrystalManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CrystalManager>();
            }
            return instance;
        }
    }
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] CrystalListEntity crystalListEntity;
    [SerializeField] CrystalCombinationListEntity crystalCombinationList;
    [SerializeField] CrystalAttackListEntity crystalAttackList;
    [SerializeField] GameObject crystalPositionParent;
    Transform[] crystalPositions;

    private void Start()
    {
        crystalPositions = crystalPositionParent.GetComponentsInChildren<Transform>();

        int max = crystalPositions.Count();
        int count = 5;
        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, max);
            int crystalType = UnityEngine.Random.Range(1, crystalListEntity.lists.Count());

            Spawn((Crystal.Type)crystalType, crystalPositions[index].position);

        }
    }

    public Crystal Init(Crystal.Type type)
    {
        foreach (var crystal in crystalListEntity.lists)
        {
            if (crystal.type == type)
            {
                return crystal;
            }
        }
        return null;
    }

    public void Spawn(Crystal.Type type, Vector3 position)
    {
        foreach (var crystal in crystalListEntity.lists)
        {
            if (crystal.type == type)
            {
                var crystalField = Instantiate(crystalPrefab, position, Quaternion.identity);
                crystalField.GetComponent<CrystalField>().SetCrystal(crystal);
            }
        }
    }

    public Sprite GetSprite(Crystal.Type type)
    {
        foreach (var crystal in crystalListEntity.lists)
        {
            if (crystal.type == type)
            {
                return crystal.sprite;
            }
        }
        return null;
    }

    public Crystal.Type GetSynthesizedCrystal(Crystal.Type sorceCrystal1, Crystal.Type sorceCrystal2)
    {
        foreach (var combination in crystalCombinationList.crystalCombinations)
        {
            if (combination.synthesisSourceCrystals.Length != 2)
            {
                continue;
            }
            if (!combination.synthesisSourceCrystals.Contains(sorceCrystal1))
            {
                continue;
            }
            if (!combination.synthesisSourceCrystals.Contains(sorceCrystal2))
            {
                continue;
            }
            //合成するクリスタルが一緒の種類かつリストのクリスタルの組み合わせが異なるならスキップ
            if (sorceCrystal1 == sorceCrystal2)
            {
                if (combination.synthesisSourceCrystals[0] != combination.synthesisSourceCrystals[1])
                {
                    continue;
                }
            }
            return combination.synthesizedCrystal;
        }
        return Crystal.Type.None;
    }

    public CrystalAttack GetCrystalAttack(Crystal.Type type)
    {
        foreach (var attack in crystalAttackList.lists)
        {
            if (attack.type == type)
            {
                return attack;
            }
        }
        return null;
    }
}
