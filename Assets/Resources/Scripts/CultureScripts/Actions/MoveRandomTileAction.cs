using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}