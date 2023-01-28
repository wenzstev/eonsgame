using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImprovedBoardGen : BoardGenAlgorithm
{
    public float precipitationDropoff = .3f;
    public float elevationModifier = .05f;

    GameObject boardObj;


    public override BoardTileRelationship CreateBoard(BoardStats bs)
    {
        HeightmapGenerator heightmapGenerator = GetComponent<HeightmapGenerator>();
        heightmapGenerator.LandRisePoint = bs.LandRisePoint;
        bs.NormalizedSeaLevel = heightmapGenerator.NormalizedSeaLevel;

        boardObj = bs.gameObject;
        // step 1: heightmap
        BoardTileRelationship perlinBoard = CreateRawBoard(boardObj, bs.Width, bs.Height, heightmapGenerator);
        bs.GetComponent<Board>().boardTileRelationship = perlinBoard;

        // step 2: temperature
        CalculateTemperatures(perlinBoard.tiles);

        // step 3: precipitation
        CalculatePrecipitation(perlinBoard.tiles);

        // step 4: determine tile types (with sprites)
        //DetermineTileTypes(perlinBoard.tiles);

        return perlinBoard;
    }


    void CalculatePrecipitation(GameObject [,] tiles)
    {
        HashSet<GameObject> passedTiles = new HashSet<GameObject>();
        HashSet<GameObject> firstPass = new HashSet<GameObject>();
        HashSet<GameObject> secondPass = new HashSet<GameObject>();

        Queue<GameObject> nextTiles = new Queue<GameObject>();

        // step 1: find all coasts
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for(int x = 0; x < tiles.GetLength(0); x++)
            {
                TileChars curTileChars = tiles[x, y].GetComponent<TileChars>();

                if (!curTileChars.isUnderwater) continue;
                


                TileLocation curTile = tiles[x, y].GetComponent<TileLocation>();


                // linq is cool 
                var neighbors = Enumerable.Range(0, 8).Select((i) => curTile.GetNeighbor((Direction)i)).Where(e => e != null);
                HashSet<GameObject> coastNeighbors = neighbors.Where(neighbor => neighbor.GetComponent<TileChars>().isUnderwater == false).Select(neighbor => neighbor.gameObject).ToHashSet();
                firstPass.UnionWith(coastNeighbors);
                curTileChars.precipitation = boardObj.GetComponent<BoardStats>().globalPrecipitation;
                curTileChars.InformAllStatsCalculated();

                passedTiles.Add(curTile.gameObject);
            }
        }

        // step two: pass in successive rings farther from the coast
        while(firstPass.Count > 0)
        {
            foreach(GameObject tileObj in firstPass)
            {
                TileLocation curTile = tileObj.GetComponent<TileLocation>();

                var neighbors = Enumerable.Range(0, 8).Select(i => curTile.GetNeighbor((Direction)i)).Where(e => e != null);

                var passedNeighbors = neighbors.Where(e => passedTiles.Contains(e));
                curTile.GetComponent<TileChars>().precipitation = passedNeighbors.Average(e => calculatePrecipitation(curTile.gameObject, e)); // actual precipitation calculation is here

                // fire event to indicate that all stats are created
                curTile.GetComponent<TileChars>().InformAllStatsCalculated();


                secondPass.UnionWith(neighbors.Where(e => !passedNeighbors.Contains(e)));
            }

            passedTiles.UnionWith(firstPass); 
            firstPass.Clear();
            firstPass.UnionWith(secondPass);
            secondPass.Clear();
        }
    }

    float calculatePrecipitation(GameObject curTile, GameObject adjacentTile)
    {
        float contributedPrecipitation;
        TileChars curTileChars = curTile.GetComponent<TileChars>();
        TileChars adjacentTileChars = adjacentTile.GetComponent<TileChars>();

        SigmoidCurve tempModifierCurve = new SigmoidCurve(1, -25, -.08f);
        float tempModifier = tempModifierCurve.GetPointOnCurve(curTileChars.temperature);
        contributedPrecipitation = adjacentTileChars.precipitation * (1 - precipitationDropoff) * tempModifier;

        

        float elevationDistance = Mathf.Max(0, curTileChars.elevation - adjacentTileChars.elevation);

        contributedPrecipitation -= elevationDistance * elevationModifier; 
        contributedPrecipitation = Mathf.Max(0, contributedPrecipitation); // can't have negative precipitation

        return contributedPrecipitation;
    }



    void CalculateTemperatures(GameObject[,] tiles)
    {
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                tiles[x,y].GetComponent<TileChars>().temperature = CalculateTileTemperature(tiles[x, y].GetComponent<TileChars>());
            }
        }
    }

    float CalculateTileTemperature(TileChars tileChars)
    {
        BoardStats boardStats = boardObj.GetComponent<BoardStats>();
        // average temperature is at middle latitudes
        float distanceFromEquator = Mathf.Abs(boardStats.equator - tileChars.y) / (float)boardStats.maxDistFromEquator;
        float tempWithoutElevation = boardStats.globalTemp - (distanceFromEquator * boardStats.tempVariance) + boardStats.tempVariance / 2;

        // TODO: mediating effects of water?

        return tempWithoutElevation - tileChars.elevation / 100; // elevation is in meters, lose 1 degree Celcius per 100 meters in height change
    }
}
