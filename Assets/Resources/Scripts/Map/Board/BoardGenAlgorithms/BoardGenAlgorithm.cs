using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardGenAlgorithm : MonoBehaviour
{
    public GameObject EmptyTile;

    public abstract BoardTileRelationship CreateBoard(BoardStats bs);

    protected BoardTileRelationship CreateRawBoard(GameObject boardObj, int sampleNumX, int sampleNumY, HeightmapGenerator heightmapGenerator)
    {
        float[,] heightmap = heightmapGenerator.CreateHeightmap(sampleNumX, sampleNumY);

        GameObject[,] Tiles = new GameObject[heightmap.GetLength(0), heightmap.GetLength(1)];
        Dictionary<GameObject, (int, int)> tileLookup = new Dictionary<GameObject, (int, int)>();

        for (int y = 0; y < sampleNumY; y++)
        {
            for (int x = 0; x < sampleNumX; x++)
            {
                Tiles[x, y] = InitializeTile(x, y, boardObj.GetComponent<Board>());
                tileLookup.Add(Tiles[x, y], (x, y));
                AddElevationAndStats(Tiles[x, y], boardObj.GetComponent<BoardStats>(), heightmap[x,y]);
            }
        }

        return new BoardTileRelationship(Tiles, tileLookup);
    }

    public GameObject InitializeTile(int x, int y, Board b)
    {
        GameObject newTile = Instantiate(EmptyTile, new Vector3(x, y, 0), Quaternion.identity);
        //Debug.Log($"Creating tile for {i}, {j}, of type {rawTileValues[i, j]}");
        newTile.transform.SetParent(b.transform);
        newTile.name = x + ", " + y;

<<<<<<< HEAD
        TileLocation tileComponent = newTile.GetComponent<TileLocation>();
=======
        Tile tileComponent = newTile.GetComponent<Tile>();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        tileComponent.board = b;
        tileComponent.id = y * b.Width + x; // unique id for each tile

        TileChars tileChars = newTile.GetComponent<TileChars>();
        tileChars.x = x;
        tileChars.y = y;

        return newTile;
    }

    public void AddElevationAndStats(GameObject tile, BoardStats boardStats, float perlinHeight)
    {
        TileChars tileChars = tile.GetComponent<TileChars>();
        tileChars.absoluteHeight = perlinHeight * boardStats.elevationRange;
    }

}
