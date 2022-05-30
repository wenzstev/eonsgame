using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject TileType;

    public BoardTileRelationship tiles;



    public int width;
    public int height;


    private void Start()
    {
        CreateBoard();

    }

    public void CreateBoard()
    {
        tiles = GetComponent<BoardInputReader>().GetBoardFromInput(width, height);
    }

    public GameObject getNeighbor(GameObject tile, Direction d)
    {
        return tiles.getNeighbor(tile, d);
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
