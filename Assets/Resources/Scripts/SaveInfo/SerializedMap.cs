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
        width = b.width;
        height = b.height;
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


[System.Serializable]
public class SerializedTile
{
    public int x;
    public int y;
    public int type;
    public int tileGroundId;
    public int tileTopId;

    public List<SerializedCulture> cultures;
    



    public SerializedTile(GameObject tile, int x, int y)
    {
        this.x = x;
        this.y = y;

        cultures = new List<SerializedCulture>();

        TileInfo ti = tile.GetComponent<TileInfo>();
        //Debug.Log(ti.tileType);
        type = (int) ti.tileType;
        //Debug.Log(type);
        tileGroundId = ti.GetComponent<TileSpriteLoader>().spriteGroundId;
        tileTopId = ti.GetComponent<TileSpriteLoader>().spriteTopId;

        foreach(Culture c in ti.cultures.Values)
        {
            cultures.Add(new SerializedCulture(c));
        }

    }
}

[System.Serializable]

public class SerializedCulture
{
    public string name;

    public SerializedColor color;
    public SerializedCultureMemory cultureMemory;

    public int population;

    public int affinity;

    public int currentState;

    public SerializedCulture(Culture culture)
    {
        name = culture.name;
        color = new SerializedColor(culture.color);
        population = culture.population;
        affinity = (int) culture.affinity;
        currentState = (int)culture.currentState;
    }

}

[System.Serializable]
public class SerializedColor
{
    public float r;
    public float g;
    public float b;

    public SerializedColor(Color c)
    {
        this.r = c.r;
        this.g = c.g;
        this.b = c.b;
    }

    public Color UnserializeColor()
    {
        return new Color(r, g, b);
    }
}

[System.Serializable]
public class SerializedCultureMemory
{
    public int previousTile;
    public int previousState;
    public string cultureParentName;
    public bool wasRepelled;

    public SerializedCultureMemory(CultureMemory cm)
    {
        previousTile = cm.previousTile.id;
        previousState = (int)cm.previousState;
        cultureParentName = cm.cultureParentName;
        wasRepelled = cm.wasRepelled;
    }


}