using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public static class OverpopulationAction 
{
    static float popLossChance = .1f;
    static int numPopLost = -1;

    public static void ExecuteTurn(CultureTurnInfo cultureTurnInfo)
    {
        if (Random.value < popLossChance)
        {
            Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, cultureTurnInfo.Culture, numPopLost));
            return;
        }
        MovePreferredTileAction.MoveToPreferredTile(cultureTurnInfo);
=======
public class OverpopulationAction : CultureTurnInfo
{
    public float popLossChance = .1f;
    public int numPopLost = -1;
    public OverpopulationAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        if (Random.value < popLossChance)
        {
            Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(this, Culture, numPopLost));
            return turn;
        }

        MovePreferredTileAction moveTile = new MovePreferredTileAction(Culture);
        return moveTile.ExecuteTurn();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }
}
