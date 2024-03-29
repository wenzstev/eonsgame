using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInputReader : MonoBehaviour
{

    public BoardGenAlgorithm bg;

    BoardStats boardStats;

    public GameObject[] tileTypes {
        get
        {
            return boardStats.tileTypes;
        }
    }

    public float offset;

    private void Start()
    {
        boardStats = GetComponent<BoardStats>();
    }

    public BoardTileRelationship GenerateBoard()
    {
        return bg.CreateBoard(GetComponent<BoardStats>());
    }

  
    public BoardTileRelationship GetBoardFromInput(int[,] rawTileValues)
    {
        return MakeTiles(rawTileValues);
    }


    public GameObject createTile(int i, int j, int type, int spriteGroundId = -1, int spriteTopId = -1)
    {
        GameObject newTile = Instantiate(tileTypes[type], new Vector3(i + (offset * i), j + (offset * j), 0), Quaternion.identity);
        newTile.transform.SetParent(transform);
        newTile.GetComponent<TileLocation>().board = GetComponent<Board>();
        newTile.name = i + ", " + j;

        newTile.GetComponent<TileSpriteLoader>().Load(spriteGroundId, spriteTopId);

        return newTile;
    }

    BoardTileRelationship MakeTiles(int[,] rawTileValues)
    {
        int height = rawTileValues.GetLength(1);
        int width = rawTileValues.GetLength(0);

        GameObject[,] boardTiles = new GameObject[width, height];

        Dictionary<GameObject, (int, int)> tileLookup = new Dictionary<GameObject, (int, int)>();



        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                //Debug.Log($"Creating tile for {i}, {j}, of type {rawTileValues[i, j]}");
                GameObject curTile = createTile(i, j, rawTileValues[i, j]);
                curTile.GetComponent<TileLocation>().id = j * width + i; // unique id for each tile
                boardTiles[i, j] = curTile;
                tileLookup.Add(curTile, (i, j));

            }
        }

        return new BoardTileRelationship(boardTiles, tileLookup);
    }

}
