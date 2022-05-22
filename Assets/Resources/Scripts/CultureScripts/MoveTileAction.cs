using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileAction : CultureMoveAction
{
    public MoveTileAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        turn.newState = AttemptMove();
        return turn;

    }

    Culture.State AttemptMove()
    {

        if (prospectiveTile == null || !(culture.affinity == prospectiveTile.GetComponent<TileInfo>().tileType || Random.value < .01))
        {
            return Culture.State.Default;
        }

        if (culture.population > culture.maxPopTransfer)
        {
            GameObject splitCulture = culture.SplitCultureFromParent();
            culture.StartCoroutine(MoveTile(splitCulture, prospectiveTile));
            splitCulture.GetComponent<Culture>().currentState = Culture.State.Moving;
            return Culture.State.Default;
        }

        culture.StartCoroutine(MoveTile(culture.gameObject, prospectiveTile));
        return Culture.State.Moving;

    }
}