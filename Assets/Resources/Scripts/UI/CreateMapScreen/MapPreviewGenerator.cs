using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapPreviewGenerator : MonoBehaviour
{
    public TMP_InputField xInput;
    public TMP_InputField yInput;

    float PADDING_CONSTANT = 100;

    public GameObject baseTile;

    List<GameObject> currentTiles;

    GridLayoutGroup gl;

    BoardGenAlgorithm bg;

    GameObject boardObj;

    float height;

    int[,] values;

    public string[] biomeTypes;

    private void Start()
    {
        currentTiles = new List<GameObject>();
        gl = GetComponent<GridLayoutGroup>();
        gl.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        bg = GetComponentInChildren<BoardGenAlgorithm>();
    }

    public void GeneratePreview()
    {
        float height = GetComponent<RectTransform>().sizeDelta.y;

        int x = int.Parse(xInput.text);
        int y = int.Parse(yInput.text);

        gl.constraintCount = x;

        float celldimensions = (height - PADDING_CONSTANT) / Mathf.Max(x, y);

        gl.cellSize = new Vector2(celldimensions, celldimensions);

        // imperfect but needed to not break everything at the moment. would like to overhaul preview system entirely
        GameObject instantiatedBoard = Instantiate(boardObj);
        BoardStats boardStats = instantiatedBoard.GetComponent<BoardStats>();
        boardStats.height = y;
        boardStats.width = x;
        boardStats.tileTypes = new GameObject[biomeTypes.Length];

        //values = bg.CreateBoard(boardStats);



        foreach (GameObject tile in currentTiles)
        {
            Destroy(tile);
        }


        for (int numY = 0; numY < y; numY++)
        {
            for(int numX = 0; numX < x; numX++)
            {
                GameObject currentTile = Instantiate(baseTile);
                currentTile.transform.parent = transform;
                //currentTile.GetComponent<Image>().color = Color.Lerp(Color.black, Color.white, (float) values[numX, numY] / 5);
                currentTiles.Add(currentTile);
            }
        }

        EventManager.TriggerEvent("MapDraftCreated", null);
    }

    public void OnSaveButtonClicked()
    {
        EventManager.TriggerEvent("MapSaved", new Dictionary<string, object> { { "mapValues", values }, { "biomes", biomeTypes } });
    }

}
