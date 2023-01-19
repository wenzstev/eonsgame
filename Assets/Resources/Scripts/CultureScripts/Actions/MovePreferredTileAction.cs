using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MovePreferredTileAction : CultureMoveAction
{
    List<GameObject> ForbiddenTiles;
    TileChars[] NeighboringTiles;

    public MovePreferredTileAction(Culture c) : base(c) { }

    protected override GameObject GetTargetTile()
    {
        /**
         * Sort tiles by affinity
         *  random sorting within affinity
         * pick first and check if it is on the no-move list
         *  if not, move there
         *  if so, try the next and repeat
         */

        NeighboringTiles = GetNeighboringTiles();
        ForbiddenTiles = SetForbiddenTiles();

        foreach((TileDrawer.BiomeType, float) affinity in GetSortedAffinities())
        {
            GameObject prospectiveTile = GetAndCheckTilesOfBiome(affinity.Item1, NeighboringTiles);
            if (prospectiveTile != null) return prospectiveTile;
        }
        return null;
    }

    List<GameObject> SetForbiddenTiles()
    {
        List<GameObject> forbiddenTiles = new List<GameObject>();
        if(Culture.GetComponent<CultureMemory>().previousTile != null) 
            forbiddenTiles.Add(Culture.GetComponent<CultureMemory>().previousTile.gameObject);     
        return forbiddenTiles;
    }

    TileChars[] GetNeighboringTiles()
    {
        return Culture.Tile.TileLocation.GetAllNeighbors().Select(t => t.GetComponent<TileChars>()).ToArray();
    }

    GameObject GetAndCheckTilesOfBiome(TileDrawer.BiomeType b, TileChars[] NeighboringTiles)
    {
        List<TileChars> TilesOfThisBiome = NeighboringTiles.Where(t => t.Biome == b).ToList();
        while (TilesOfThisBiome.Count > 0)
        {
            TileChars current = TilesOfThisBiome[Mathf.FloorToInt(Random.value * TilesOfThisBiome.Count)];
            if (!IsForbiddenTile(current.gameObject)) return current.gameObject;
            TilesOfThisBiome.Remove(current);
        }
        return null;
    }

    bool IsForbiddenTile(GameObject tile)
    {
        return ForbiddenTiles.Contains(tile);
    }


    (TileDrawer.BiomeType, float)[] GetSortedAffinities()
    {
        return Culture.GetComponent<AffinityManager>().GetAllAffinities()
            .OrderByDescending(kvp => kvp.Item2).ToArray();
    }


    public override Turn ExecuteTurn()
    {
        return ExecuteMove();
    }


}
