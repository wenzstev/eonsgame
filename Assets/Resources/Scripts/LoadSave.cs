using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSave : MonoBehaviour
{
    public GameObject boardObject;

    // Start is called before the first frame update
    void Start()
    {
        string saveLocation = Application.persistentDataPath + "/gamedata.json";
        Save save = Save.UnserializeSave(saveLocation);

        Board b = boardObject.GetComponent<Board>();

        b.width = save.width;
        b.height = save.height;

        b.CreateTilesFromSerializedData(save.tiles);

    }

}
