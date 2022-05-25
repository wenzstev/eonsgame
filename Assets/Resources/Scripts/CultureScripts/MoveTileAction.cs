using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileAction : CultureMoveAction
{
    public MoveTileAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        return AttemptMove();

    }

    Turn AttemptMove()
    {

        if (prospectiveTile == null || !(culture.affinity == prospectiveTile.GetComponent<TileInfo>().tileType || Random.value < .01))
        {
            turn.UpdateCulture(culture).newState = Culture.State.Default;
            return turn;
        }

        if (culture.population > culture.maxPopTransfer)
        {
            GameObject splitCultureObj = culture.SplitCultureFromParent();
            Culture splitCulture = splitCultureObj.GetComponent<Culture>();

            splitCulture.StartCoroutine(MoveTile(splitCultureObj, prospectiveTile));
            turn.UpdateCulture(splitCulture).newState = Culture.State.Moving;
            turn.UpdateCulture(culture).newState = Culture.State.Default;

            return turn;
        }

        culture.StartCoroutine(MoveTile(culture.gameObject, prospectiveTile));
        turn.UpdateCulture(culture).newState = Culture.State.Moving;
        return turn;

    }
}