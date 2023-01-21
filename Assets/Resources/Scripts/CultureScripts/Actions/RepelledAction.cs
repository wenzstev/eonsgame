using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public static class RepelledAction
{

    public static void RepelCulture(CultureTurnInfo cultureTurnInfo)
    {
        Culture culture = cultureTurnInfo.Culture;
        if(culture.CultureMemory.wasRepelled || culture.CultureMemory.previousTile == null)
        {
            WasPreviouslyRepelled(cultureTurnInfo);
            return;
        }

        ReturnToPreviousTile(cultureTurnInfo);
    }

    static Tile GetTargetTile(Culture c)
    {
        return c.CultureMemory.previousTile;
    }

    static void ReturnToPreviousTile(CultureTurnInfo cultureTurnInfo)
    {
        //Debug.Log(culture.GetComponent<CultureMemory>().previousTile);

        Turn.AddUpdate(CultureUpdateGetter.GetMoveUpdate(cultureTurnInfo, cultureTurnInfo.Culture, GetTargetTile(cultureTurnInfo.Culture)));
    }

    static void WasPreviouslyRepelled(CultureTurnInfo cultureTurnInfo)
    {
        MoveRandomTileAction.MoveRandomTile(cultureTurnInfo);
=======
public class RepelledAction : CultureMoveAction
{
    public RepelledAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        if(Culture.GetComponent<CultureMemory>().wasRepelled)
        {
            return WasPreviouslyRepelled();
        }

        return ReturnToPreviousTile();
    }

    protected override GameObject GetTargetTile()
    {
        return Culture.GetComponent<CultureMemory>().previousTile.gameObject;
    }

    Turn ReturnToPreviousTile()
    {
        //Debug.Log(culture.GetComponent<CultureMemory>().previousTile);

        return ExecuteMove();
    }

    Turn WasPreviouslyRepelled()
    {
        MoveRandomTileAction mta = new MoveRandomTileAction(Culture);
        return mta.ExecuteTurn();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }
 
}
