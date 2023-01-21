using System.Linq;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public static class MovePreferredTileAction
{
    static Tile GetTargetTile(CultureTurnInfo cultureTurnInfo)
=======
public class MovePreferredTileAction : CultureMoveAction
{
    List<GameObject> ForbiddenTiles;
    TileChars[] NeighboringTiles;

    public MovePreferredTileAction(Culture c) : base(c) { }

    protected override GameObject GetTargetTile()
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    {
        /**
         * Sort tiles by affinity
         *  random sorting within affinity
         * pick first and check if it is on the no-move list
         *  if not, move there
         *  if so, try the next and repeat
         */

<<<<<<< HEAD
        Culture culture = cultureTurnInfo.Culture;

        Tile[] NeighboringTiles = GetNeighboringTiles(culture);
        List<GameObject> ForbiddenTiles = SetForbiddenTiles(culture);

        foreach((TileDrawer.BiomeType, float) affinity in GetSortedAffinities(culture))
        {
            Tile prospectiveTile = GetAndCheckTilesOfBiome(affinity.Item1, NeighboringTiles, ForbiddenTiles);
=======
        NeighboringTiles = GetNeighboringTiles();
        ForbiddenTiles = SetForbiddenTiles();

        foreach((TileDrawer.BiomeType, float) affinity in GetSortedAffinities())
        {
            GameObject prospectiveTile = GetAndCheckTilesOfBiome(affinity.Item1, NeighboringTiles);
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
            if (prospectiveTile != null) return prospectiveTile;
        }
        return null;
    }

<<<<<<< HEAD
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
=======
    List<GameObject> SetForbiddenTiles()
    {
        List<GameObject> forbiddenTiles = new List<GameObject>();
        if(Culture.GetComponent<CultureMemory>().previousTile != null) 
            forbiddenTiles.Add(Culture.GetComponent<CultureMemory>().previousTile.gameObject);     
        return forbiddenTiles;
    }

    TileChars[] GetNeighboringTiles()
    {
        return Culture.Tile.GetAllNeighbors().Select(t => t.GetComponent<TileChars>()).ToArray();
    }

    GameObject GetAndCheckTilesOfBiome(TileDrawer.BiomeType b, TileChars[] NeighboringTiles)
    {
        List<TileChars> TilesOfThisBiome = NeighboringTiles.Where(t => t.Biome == b).ToList();
        while (TilesOfThisBiome.Count > 0)
        {
            TileChars current = TilesOfThisBiome[Mathf.FloorToInt(Random.value * TilesOfThisBiome.Count)];
            if (!IsForbiddenTile(current.gameObject)) return current.gameObject;
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
            TilesOfThisBiome.Remove(current);
        }
        return null;
    }

<<<<<<< HEAD

    static (TileDrawer.BiomeType, float)[] GetSortedAffinities(Culture culture)
    {
        return culture.GetComponent<AffinityManager>().GetAllAffinities()
=======
    bool IsForbiddenTile(GameObject tile)
    {
        return ForbiddenTiles.Contains(tile);
    }


    (TileDrawer.BiomeType, float)[] GetSortedAffinities()
    {
        return Culture.GetComponent<AffinityManager>().GetAllAffinities()
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
            .OrderByDescending(kvp => kvp.Item2).ToArray();
    }


<<<<<<< HEAD
    public static void MoveToPreferredTile(CultureTurnInfo cultureTurnInfo)
    {
        Tile targetTile = GetTargetTile(cultureTurnInfo);
        Turn.AddUpdate(CultureUpdateGetter.GetMoveUpdate(cultureTurnInfo, cultureTurnInfo.Culture, targetTile));
=======
    public override Turn ExecuteTurn()
    {
        return ExecuteMove();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }


}
