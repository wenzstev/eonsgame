using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileAction : CultureMoveAction
{
    public float moveChance = .01f; // no more need for a movechance either

    public MoveTileAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        return AttemptMove();

    }

    Turn AttemptMove()
    {
        if (prospectiveTile == null || prospectiveTile.GetComponent<TileChars>().Biome == TileDrawer.BiomeType.Water) // this is doing way too much work
        {
            turn.UpdateCulture(Culture).newState = Culture.State.Default;

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
        turn.UpdateCulture(splitCulture).newState = Culture.State.Moving;
        turn.UpdateCulture(Culture).newState = Culture.State.Default;
        return turn;
    }

    Turn MoveWholeCulture()
    {
        Culture.StartCoroutine(MoveTile(Culture.gameObject, prospectiveTile));
        turn.UpdateCulture(Culture).newState = Culture.State.Moving;
        return turn;
    }
}