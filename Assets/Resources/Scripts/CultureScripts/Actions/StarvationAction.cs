using System.Collections;
using UnityEngine;

<<<<<<< HEAD
public static class StarvationAction
{

    public static float PopLossChance = .5f;
    public static int NumPopLost = 1;


    public static void ExecuteTurn(CultureTurnInfo cultureTurnInfo)
    {
        if(Random.value > PopLossChance)
        {
            Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, cultureTurnInfo.Culture, -NumPopLost));
        }
        MoveRandomTileAction.MoveRandomTile(cultureTurnInfo);
=======
public class StarvationAction : CultureTurnInfo
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
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }

}
