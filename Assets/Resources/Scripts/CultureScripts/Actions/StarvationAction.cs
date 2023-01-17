using System.Collections;
using UnityEngine;

public class StarvationAction : CultureAction
{
    public StarvationAction(Culture c) : base(c) { }

    public float PopLossChance = .5f;
    public int NumPopLost = 1;


    public override Turn ExecuteTurn()
    {
        if(Random.value > PopLossChance)
        {
            Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(this, Culture, -NumPopLost));
        }
        MoveRandomTileAction randomMove = new MoveRandomTileAction(Culture);
        return randomMove.ExecuteTurn();
    }

}
