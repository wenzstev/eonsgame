using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LoadSave : MonoBehaviour
{
    public GameObject boardObject;
    public CultureLoader cl;

    public bool DEBUG_LOAD_DEFAULT;

    Save save;

    // Start is called before the first frame update
    void Start()
    {
        save = DEBUG_LOAD_DEFAULT ? LoadDefault() : LoadFromScene();

        Board b = boardObject.GetComponent<Board>();

        b.width = save.width;
        b.height = save.height;

        b.CreateTilesFromSerializedData(save.tiles);
        cl.CreateCultures(save.tiles);
    }

    private void OnDestroy()
    {
        foreach(SaveObject saveObj in FindObjectsOfType<SaveObject>())
        {
            Destroy(saveObj);
        }
    }

    Save LoadFromScene()
    {
        SaveObject[] saveObj = FindObjectsOfType<SaveObject>();
        if (saveObj.Length != 1)
        {
            throw new ArgumentException("Must have one save instance in scene!");
        }

       return saveObj[0].save;
    }

    Save LoadDefault()
    {
        string loadSave = $"{Application.persistentDataPath}/untitled.json";
        return Save.UnserializeSave(loadSave);
    }

}
