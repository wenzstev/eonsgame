using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInputReader : MonoBehaviour
{

    public BoardGenerator bg;

    public GameObject[] tiles;

    public float offset;



    public BoardTileRelationship GetBoardFromInput(int width, int height)
    {
        GameObject[,] boardTiles = new GameObject[width, height];
        Dictionary<GameObject, (int, int)> tileLookup = new Dictionary<GameObject, (int, int)>();

        int[,] rawTileValues = bg.getLevelledBoard(tiles.Length, width, height);


        for (int j= 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                boardTiles[i, j] = Instantiate(tiles[rawTileValues[i, j]], new Vector3(i + (offset * i), j + (offset * j), 0), Quaternion.identity);
                boardTiles[i, j].transform.SetParent(transform);
                boardTiles[i, j].GetComponent<Tile>().board = GetComponent<Board>();
                tileLookup.Add(boardTiles[i, j], (i, j));
                boardTiles[i, j].name = i + ", " + j;
                
            }
        }

        return new BoardTileRelationship(boardTiles, tileLookup);
    }

}
