using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
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
=======
public class MoveRandomTileAction : CultureMoveAction
{
    public float moveChance = .01f; // no more need for a movechance either

    public MoveRandomTileAction(Culture c) : base(c) {}

    public override Turn ExecuteTurn()
    {
        return Culture.isActiveAndEnabled ? AttemptMove() : turn;
    }

    protected override GameObject GetTargetTile()
    {
        return Culture.Tile.TileLocation.GetRandomNeighbor();
    }

    Turn AttemptMove()
    {
        if (TargetTile == null || TargetTile.GetComponent<TileChars>().Biome == TileDrawer.BiomeType.Water) // this is doing way too much work
        {
            Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, Culture, Culture.State.Default));

            return turn;
        }

        return ExecuteMove();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }
}