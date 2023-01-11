using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileAction : CultureMoveAction
{
    public float moveChance = .01f; // no more need for a movechance either

    public MoveTileAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        return Culture.isActiveAndEnabled ? AttemptMove() : turn;

    }

    Turn AttemptMove()
    {
        if (prospectiveTile == null || prospectiveTile.GetComponent<TileChars>().Biome == TileDrawer.BiomeType.Water) // this is doing way too much work
        {
            Turn.AddUpdate(new StateUpdate(this, Culture, Culture.State.Default));

            return turn;
        }

        if (Culture.Population > Culture.maxPopTransfer)
        {
            return MoveSplitCulture(); ;
        }
        return MoveWholeCulture();
    }

    Turn MoveSplitCulture()
    {
        GameObject splitCultureObj = Culture.SplitCultureFromParent();
        Culture splitCulture = splitCultureObj.GetComponent<Culture>();
        splitCulture.StartCoroutine(MoveTile(splitCulture.gameObject, prospectiveTile));
        Turn.AddUpdate(new StateUpdate(this, splitCulture, Culture.State.Moving));
        Turn.AddUpdate(new StateUpdate(this, Culture, Culture.State.Default));
        return turn;
    }

    Turn MoveWholeCulture()
    {
        Culture.StartCoroutine(MoveTile(Culture.gameObject, prospectiveTile));
        Turn.AddUpdate(new StateUpdate(this, Culture, Culture.State.Moving));
        return turn;
    }
}