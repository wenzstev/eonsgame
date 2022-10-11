using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<SerializedTile> tiles;
    public int width;
    public int height;

    public Save(Board b)
    {
        width = b.Width;
        height = b.Height;
        tiles = new List<SerializedTile>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Debug.Log(b.tiles.GetTile(x, y));
                tiles.Add(new SerializedTile(b.tiles.GetTile(x, y), x, y));
            }
        }
    }

    public Board getBoard()
    {
        return null;
    }

    public static void SerializeSave(Save saveData, string saveName)
    {
        string saveFileLocation = $"{Application.persistentDataPath}/{saveName}.json";
        Debug.Log(saveFileLocation);

        string saveFileJson = JsonUtility.ToJson(saveData);
        Debug.Log(saveFileJson);
        File.WriteAllText(saveFileLocation, saveFileJson);

    }

    public static Save UnserializeSave(string savePath)
    {
        string saveJson = File.ReadAllText(savePath);
        return JsonUtility.FromJson<Save>(saveJson);
    }

    public static GameObject CreatePersistantSave(Save save)
    {
        GameObject saveObj = new GameObject("SaveFile");
        SaveObject so = saveObj.AddComponent<SaveObject>();
        so.InitializeSave(save);
        return saveObj;
    }
}






