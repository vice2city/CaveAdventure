using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameProcess
{
    public bool[] caveState;
    public bool[] skillState;
    public int spareNum;
    public List<Vector2Int> openedBox;
    public List<Vector2Int> unlockedDoor;
    public List<Vector2Int> usedKey;
    public bool isGameEasy;
}

public class DataManager : MonoBehaviour
{
    public void SaveGameData()
    {
        var process = GameManager.instance.GetGameData();
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
        var process = new GameProcess();
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
}
