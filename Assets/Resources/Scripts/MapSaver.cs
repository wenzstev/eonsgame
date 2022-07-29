using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSaver : MonoBehaviour
{

    public GameObject board;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("MapSaved", OnMapSaved);    
    }

    
    void OnMapSaved(Dictionary<string, object> mapData)
    {
        int[,] values = (int[,]) mapData["mapValues"];


        GameObject bObject = Instantiate(board);
        Board b = bObject.GetComponent<Board>();

        b.width = values.GetLength(0);
        b.height = values.GetLength(1);

        b.CreateBoardFromValues(values);


        Save save = new Save(b);
        Save.SerializeSave(save, "untitled");


    }
}
