using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileAction : CultureMoveAction
{
    public float moveChance = .01f;

    public MoveTileAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        return AttemptMove();

    }

    Turn AttemptMove()
    {

        if (prospectiveTile == null || prospectiveTile.GetComponent<TileInfo>().tileType == TileDrawer.BiomeType.Water || !(culture.affinity == prospectiveTile.GetComponent<TileInfo>().tileType || Random.value < moveChance))
        {
            turn.UpdateCulture(culture).newState = Culture.State.Default;

            return turn;
        }

        if (culture.Population > culture.maxPopTransfer)
        {
            return MoveSplitCulture(); ;
        }
        return MoveWholeCulture();
    }

    Turn MoveSplitCulture()
    {
        GameObject splitCultureObj = culture.SplitCultureFromParent();
        Culture splitCulture = splitCultureObj.GetComponent<Culture>();
        splitCulture.StartCoroutine(MoveTile(splitCulture.gameObject, prospectiveTile));
        turn.UpdateCulture(splitCulture).newState = Culture.State.Moving;
        turn.UpdateCulture(culture).newState = Culture.State.Default;
        return turn;
    }

    Turn MoveWholeCulture()
    {
        culture.StartCoroutine(MoveTile(culture.gameObject, prospectiveTile));
        turn.UpdateCulture(culture).newState = Culture.State.Moving;
        return turn;
    }
}