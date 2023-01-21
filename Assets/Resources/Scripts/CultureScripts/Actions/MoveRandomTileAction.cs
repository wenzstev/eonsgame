using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveRandomTileAction
{

    public static void MoveRandomTile(CultureTurnInfo cultureTurnInfo)
    {
        Culture culture = cultureTurnInfo.Culture;
        if(culture.isActiveAndEnabled) AttemptMove(cultureTurnInfo);
    }

    static Tile GetTargetTile(Culture c)
    {
        return c.Tile.TileLocation.GetRandomNeighbor().GetComponent<Tile>(); // kinda gross
    }

    static void AttemptMove(CultureTurnInfo cultureTurnInfo)
    {
        Culture culture = cultureTurnInfo.Culture;
        Tile TargetTile = GetTargetTile(culture);
        if (TargetTile == null || TargetTile.GetComponent<TileChars>().Biome == TileDrawer.BiomeType.Water) // this is doing way too much work
        {
            Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, culture, Culture.State.Default));
            return;
        }

        Turn.AddUpdate(CultureUpdateGetter.GetMoveUpdate(cultureTurnInfo, culture, TargetTile));
    }
}