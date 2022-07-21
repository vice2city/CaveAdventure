using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameProcess
{
    public bool[] caveState;
    public List<Vector2Int> unlockedDoor;
    public List<Vector2Int> usedKey;
}

public class DataManager : MonoBehaviour
{
    public GameProcess process = new GameProcess();
    
    public void SaveGameData()
    {
        RefreshData();
        var dataPath = Application.persistentDataPath + "/game_SaveData";
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }

        var formatter = new BinaryFormatter();
        var file = File.Create(dataPath + "/GameManager.data");
        var json = JsonUtility.ToJson(process);
        formatter.Serialize(file, json);
        file.Close();
        Debug.Log("Game data is saved on "+dataPath);
    }

    public void LoadGameData()
    {
        var dataPath = Application.persistentDataPath + "/game_SaveData/GameManager.data";
        if (!File.Exists(dataPath)) return;
        
        var file = File.Open(dataPath, FileMode.Open);
        var formatter = new BinaryFormatter();
        var json = formatter.Deserialize(file);
        JsonUtility.FromJsonOverwrite(json.ToString(), process);
        GameManager.instance.LoadGameData(process);
        file.Close();
        Debug.Log("Game data is loaded with "+dataPath);
    }

    private void RefreshData()
    {
        process.unlockedDoor = GameManager.instance.unlockedDoor;
        process.usedKey = GameManager.instance.usedKey;
        process.caveState = GameManager.instance.caveState;
    }
}
