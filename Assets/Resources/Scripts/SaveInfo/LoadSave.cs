using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSave : MonoBehaviour
{
    public GameObject boardObject;
    public CultureLoader cl;

    // Start is called before the first frame update
    void Start()
    {
        SaveObject[] saveObj = FindObjectsOfType<SaveObject>();
        if (saveObj.Length != 1)
        {
            Debug.LogError("Incorrect number of saves in scene.");
            return;
        }

        Save save = saveObj[0].save;

        Board b = boardObject.GetComponent<Board>();

        b.width = save.width;
        b.height = save.height;

        b.CreateTilesFromSerializedData(save.tiles);
        cl.CreateCultures(save.tiles);

        Destroy(saveObj[0]);

    }

}
