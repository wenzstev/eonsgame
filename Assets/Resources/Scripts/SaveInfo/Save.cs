using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Save
{
    public SerializedBoard sBoard;
    public SerializedTiles sTiles;
    public SerializedCultures sCultures;


    public Save(Board b)
    {
        sBoard = new SerializedBoard(b);
        sTiles = new SerializedTiles(b);
        sCultures = new SerializedCultures(b);
    }

    public static void SerializeSave(Save saveData, string saveName)
    {
        string saveFileLocation = $"{Application.persistentDataPath}/{saveName}.json";
        Debug.Log(saveFileLocation);

        //Directory.CreateDirectory(saveFileLocation);

        string saveFileBoard = JsonUtility.ToJson(saveData.sBoard);
        string saveFileTiles = JsonUtility.ToJson(saveData.sTiles);
        string saveFileCultures = JsonUtility.ToJson(saveData.sCultures);

        string saveFileJson = JsonUtility.ToJson(saveData);

        File.WriteAllText($"{saveFileLocation}", saveFileJson);   

        //File.WriteAllText($"{saveFileLocation}/board.json", saveFileBoard);
        //File.WriteAllText($"{saveFileLocation}/tiles.json", saveFileTiles);
        //File.WriteAllText($"{saveFileLocation}/cultures.json", saveFileCultures);
    }

    public static Save UnserializeSave(string saveName)
    {
        //string saveBoard = File.ReadAllText($"{savePath}/board.json");
        //string saveTiles = File.ReadAllText($"{savePath}/tiles.json");
        //string saveCultures = File.ReadAllText($"{savePath}/cultures.json");

        string savePath = $"{Application.persistentDataPath}/{saveName}.json";

        string saveJson = File.ReadAllText(savePath);

        return JsonUtility.FromJson<Save>(saveJson);
    }

    public static GameObject CreatePersistantSave(Save save, string saveName)
    {
        GameObject saveObj = new GameObject("SaveFile");
        SaveObject so = saveObj.AddComponent<SaveObject>();
        so.InitializeSave(saveName);
        return saveObj;
    }
}






