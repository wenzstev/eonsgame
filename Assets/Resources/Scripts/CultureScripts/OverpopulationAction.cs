using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverpopulationAction : CultureAction
{
    public OverpopulationAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        CultureAction moveTile = new MoveTileAction(culture);
        return moveTile.ExecuteTurn();


    }
}
