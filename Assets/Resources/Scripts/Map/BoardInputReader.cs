using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInputReader : MonoBehaviour
{

    public BoardGenerator bg;

    public GameObject[] tileTypes;

    public float offset;

    public BoardTileRelationship GetBoardFromInput(int width, int height)
    {

        int[,] rawTileValues = bg.getLevelledBoard(tileTypes.Length, width, height);

        return MakeTiles(rawTileValues);
        
    }

    public BoardTileRelationship GetBoardFromInput(int[,] rawTileValues)
    {
        return MakeTiles(rawTileValues);
    }

    public BoardTileRelationship GetBoardFromSerializedTiles(List<SerializedTile> tiles, int height, int width)
    {

        GameObject[,] boardTiles = new GameObject[width, height];
        Dictionary<GameObject, (int, int)> tileLookup = new Dictionary<GameObject, (int, int)>();

        int i = 0;
        int j = 0;

        foreach(SerializedTile t in tiles)
        {
            //Debug.Log(t.x + "  " + t.y + "  " +  t.type);
            GameObject curTile = createTile(i, j, t.type, t.tileId);
            boardTiles[i, j] = curTile;
            tileLookup.Add(curTile, (i, j));
            curTile.GetComponent<Tile>().id = j * width + 1;

            i++;
            if(i >= width)
            {
                i = 0;
                j++;
            }
        }

        return new BoardTileRelationship(boardTiles, tileLookup);
    }

    public GameObject createTile(int i, int j, int type, int tileId=-1)
    {
        GameObject newTile = Instantiate(tileTypes[type], new Vector3(i + (offset * i), j + (offset * j), 0), Quaternion.identity);
        newTile.transform.SetParent(transform);
        newTile.GetComponent<Tile>().board = GetComponent<Board>();
        newTile.name = i + ", " + j;

        newTile.GetComponent<TileSpriteLoader>().Load(tileId);

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
                //Debug.Log(rawTileValues[i, j]);
                GameObject curTile = createTile(i, j, rawTileValues[i, j]);
                curTile.GetComponent<Tile>().id = j * width + i; // unique id for each tile
                boardTiles[i, j] = curTile;
                tileLookup.Add(curTile, (i, j));

            }
        }

        return new BoardTileRelationship(boardTiles, tileLookup);
    }

}
