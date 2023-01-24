using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class MovePreferredTileAction
{
    static Tile GetTargetTile(CultureTurnInfo cultureTurnInfo)
    {
        /**
         * Sort tiles by affinity
         *  random sorting within affinity
         * pick first and check if it is on the no-move list
         *  if not, move there
         *  if so, try the next and repeat
         */

        Culture culture = cultureTurnInfo.Culture;

        Tile[] NeighboringTiles = GetNeighboringTiles(culture);
        List<GameObject> ForbiddenTiles = SetForbiddenTiles(culture);

        foreach((TileDrawer.BiomeType, float) affinity in GetSortedAffinities(culture))
        {
            Tile prospectiveTile = GetAndCheckTilesOfBiome(affinity.Item1, NeighboringTiles, ForbiddenTiles);
            if (prospectiveTile != null) return prospectiveTile;
        }
        return null;
    }

    static List<GameObject> SetForbiddenTiles(Culture culture)
    {
        List<GameObject> forbiddenTiles = new List<GameObject>();
        if(culture.CultureMemory.previousTile != null) 
            forbiddenTiles.Add(culture.GetComponent<CultureMemory>().previousTile.gameObject);     
        return forbiddenTiles;
    }

    static Tile[] GetNeighboringTiles(Culture culture)
    {
        return culture.Tile.TileLocation.GetAllNeighbors().Select(t => t.GetComponent<Tile>()).ToArray();
    }

    static Tile GetAndCheckTilesOfBiome(TileDrawer.BiomeType b, Tile[] NeighboringTiles, List<GameObject> ForbiddenTiles)
    {
        List<Tile> TilesOfThisBiome = NeighboringTiles.Where(t => t.TileChars.Biome == b).ToList();
        while (TilesOfThisBiome.Count > 0)
        {
            Tile current = TilesOfThisBiome[Mathf.FloorToInt(Random.value * TilesOfThisBiome.Count)];
            if (!ForbiddenTiles.Contains(current.gameObject)) return current;
            TilesOfThisBiome.Remove(current);
        }
        return null;
    }


    static (TileDrawer.BiomeType, float)[] GetSortedAffinities(Culture culture)
    {
        return culture.GetComponent<AffinityManager>().GetAllAffinities()
            .OrderByDescending(kvp => kvp.Item2).ToArray();
    }


    public static void MoveToPreferredTile(CultureTurnInfo cultureTurnInfo)
    {
        Tile targetTile = GetTargetTile(cultureTurnInfo);
        Turn.AddTileUpdate(CultureUpdateGetter.GetMoveUpdate(cultureTurnInfo, cultureTurnInfo.Culture, targetTile));
    }


}
