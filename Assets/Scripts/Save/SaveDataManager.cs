using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class SaveDataManager
{
    private static SaveDataManager instance;
    public static SaveDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveDataManager();
            }
            return instance;
        }
    }
    private SaveData saveData;
    public SaveData SaveData
    {
        get
        {
            if (saveData == null)
            {
                Load();
            }
            return saveData;
        }
    }
    string fileName;
    private SaveDataManager()
    {
        fileName = Application.persistentDataPath + "/savedata";
    }

    public void Save()
    {
        binarySaveLoad.Save(fileName, saveData);
    }

    public void Load()
    {
        saveData = new SaveData();
        if (File.Exists(fileName))
        {
            binarySaveLoad.Load(fileName, out saveData);
        }
        else
        {
            DataReset();
        }
    }

    public void DataReset()
    {
        saveData = new SaveData();
        Save();
    }
}
