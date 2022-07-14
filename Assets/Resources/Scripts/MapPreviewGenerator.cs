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

    BoardGenerator bg;

    float height;

    private void Start()
    {
        currentTiles = new List<GameObject>();
        gl = GetComponent<GridLayoutGroup>();
        gl.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        bg = GetComponentInChildren<BoardGenerator>();

    }

    public void GeneratePreview()
    {
        float height = GetComponent<RectTransform>().sizeDelta.y;

        int x = int.Parse(xInput.text);
        int y = int.Parse(yInput.text);

        gl.constraintCount = x;

        float celldimensions = (height - PADDING_CONSTANT) / Mathf.Max(x, y);

        gl.cellSize = new Vector2(celldimensions, celldimensions);


        int[,] coords = bg.getLevelledBoard(5, x, y);



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
                currentTile.GetComponent<Image>().color = Color.Lerp(Color.black, Color.white, (float) coords[numX, numY] / 5);
                currentTiles.Add(currentTile);
            }
        }

        EventManager.TriggerEvent("MapDraftCreated", null);
    }
}
