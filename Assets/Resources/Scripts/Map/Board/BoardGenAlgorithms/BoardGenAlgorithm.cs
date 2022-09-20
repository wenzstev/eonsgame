using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardGenAlgorithm : MonoBehaviour
{
    public GameObject EmptyTile;


    public abstract BoardTileRelationship CreateBoard(BoardStats bs);

    protected BoardTileRelationship CreateRawPerlinBoard(GameObject boardObj, float scale, int sampleNumX, int sampleNumY)
    {
        GameObject[,] Tiles = new GameObject[sampleNumX, sampleNumY];
        Dictionary<GameObject, (int, int)> tileLookup = new Dictionary<GameObject, (int, int)>();


        Vector2 startPosition = new Vector2(Random.value * 10, Random.value * 10);
        float[,] points = new float[sampleNumX, sampleNumY];
        for (int y = 0; y < sampleNumY; y++)
        {
            for (int x = 0; x < sampleNumX; x++)
            {
                float currentSampleX = Mathf.Lerp(startPosition.x, startPosition.x + scale, Mathf.InverseLerp(0, sampleNumX, x));
                float currentSampleY = Mathf.Lerp(startPosition.y, startPosition.y + scale, Mathf.InverseLerp(0, sampleNumY, y));
                float curHeightPoint = Mathf.PerlinNoise(currentSampleX, currentSampleY);
                Tiles[x, y] = InitializeTile(x, y, boardObj.GetComponent<Board>());
                tileLookup.Add(Tiles[x, y], (x, y));

                AddElevationAndStats(Tiles[x, y], boardObj.GetComponent<BoardStats>(), curHeightPoint);
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

        Tile tileComponent = newTile.GetComponent<Tile>();
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
        tileChars.boardStats = boardStats;
        tileChars.absoluteHeight = perlinHeight * boardStats.elevationRange;
    }

}
