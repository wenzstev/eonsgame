using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        BoardStats boardStats = bObject.GetComponent<BoardStats>();
        boardStats.SetDimensions(values.GetLength(0), values.GetLength(1));
        boardStats.SetTileWidth(1); // hard coded right now, need to change to get from spite size

        b.CreateBoardFromValues(values);


        Save save = new Save(b);
        Save.SerializeSave(save, "untitled");
        Save.CreatePersistantSave(save, "untitled");
        SceneManager.LoadScene("PlayScene");
    }
}
