using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public BoardTileRelationship tiles;

    public int width;
    public int height;

    public bool DEBUG_TEST_GEN = false;

    private void Start()
    {
        if(DEBUG_TEST_GEN)
        {
            CreateBoard();
        }
    }

    public void CreateBoard()
    {
        tiles = GetComponent<BoardInputReader>().GetBoardFromInput(width, height);
    }

    public void CreateBoardFromValues(int[,] values)
    {
        tiles = GetComponent<BoardInputReader>().GetBoardFromInput(values);
    }

    public GameObject getNeighbor(GameObject tile, Direction d)
    {
        return tiles.getNeighbor(tile, d);
    }

    public GameObject GetTileByID(int id)
    {
        if(id > width * height - 1)
        {
            throw new System.Exception("Tried to get ID that is too big for board!");
        }

        int xpos = id;
        int ypos = 0;
        while(xpos > width)
        {
            xpos -= width;
            ypos += 1;
        }

        return tiles.GetTile(xpos, ypos);
    }

    public void CreateTilesFromSerializedData(List<SerializedTile> tiles)
    {
        this.tiles = GetComponent<BoardInputReader>().GetBoardFromSerializedTiles(tiles, height, width);
    }

    public GameObject GetTile(int x, int y)
    {
        return tiles.GetTile(x, y);
    }

}

public class BoardTileRelationship
{
    GameObject[,] tiles;
    public Dictionary<GameObject, (int, int)> tileLookup;

    static (int, int)[] directionToCoords = new (int, int)[]
    {
        (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)
    };

    public BoardTileRelationship(GameObject[,] t, Dictionary<GameObject, (int,  int)> tl)
    {
        tiles = t;
        tileLookup = tl;
    }

    public GameObject getNeighbor(GameObject tile, Direction d)
    {   
        (int x, int y) coords;
        if(tileLookup.TryGetValue(tile, out coords))
        {
            (int x, int y) directionModifier = directionToCoords[(int)d];
            int checkX = coords.x + directionModifier.x;
            int checkY = coords.y + directionModifier.y;


            if (checkX >= 0 && checkY >= 0 && checkX < tiles.GetLength(0) && checkY < tiles.GetLength(1))
            {
                return tiles[checkX, checkY];
            }
        }
        return null;
    }

    public GameObject GetTile(int x, int y)
    {
        return tiles[x, y];
    }
}
