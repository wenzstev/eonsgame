using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public BoardTileRelationship boardTileRelationship;



    public GameObject[,] Tiles
    {
        get
        {
            return boardTileRelationship.tiles;
        }
    }

    public int Width { 
        get
        {
            return boardStats.Width;
        }
    }
    public int Height { 
        get
        {
            return boardStats.Height;
        }
    }

    public bool DEBUG_TEST_GEN = false;

    BoardStats boardStats;

    private void Awake()
    {
        boardStats = GetComponent<BoardStats>();
        if(DEBUG_TEST_GEN) CreateBoard();
    }


    public void SetBoardTiles(Dictionary<string, object> boardDict)
    {
        boardTileRelationship = (BoardTileRelationship) boardDict["tiles"];
        EventManager.StopListening("BoardCreated", SetBoardTiles);

    }

    public void CreateBoard()
    {
        boardTileRelationship = GetComponent<BoardInputReader>().GenerateBoard();
    }

    public void CreateBoardFromValues(int[,] values)
    {
        boardTileRelationship = GetComponent<BoardInputReader>().GetBoardFromInput(values);
    }

    public GameObject getNeighbor(GameObject tile, Direction d)
    {
        return boardTileRelationship.getNeighbor(tile, d);
    }

    public GameObject GetTileByID(int id)
    {
        if(id > Width * Height - 1)
        {
            throw new System.Exception("Tried to get ID that is too big for board!");
        }

        int xpos = id;
        int ypos = 0;
        while(xpos > Width)
        {
            xpos -= Width;
            ypos += 1;
        }

        return boardTileRelationship.GetTile(xpos, ypos);
    }


    public GameObject GetTile(int x, int y)
    {
        return boardTileRelationship.GetTile(x, y);
    }
}

public class BoardTileRelationship
{
    public GameObject[,] tiles;
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
