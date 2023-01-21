using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public static class AttemptRepelAction
{

    public static void AttemptRepel(CultureTurnInfo cultureTurnInfo)
    {
        Culture culture = cultureTurnInfo.Culture;
        foreach (Culture c in culture.Tile.GetComponentInChildren<CultureHandler>().GetAllSettledCultures())
        {
            if (c.currentState == Culture.State.Invader)
            {
                bool didRepel = AttemptRepelInvader(cultureTurnInfo, culture);
                if (didRepel) return;
            }
        }
        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, culture, Culture.State.Default));
    }

    static bool AttemptRepelInvader(CultureTurnInfo cultureTurnInfo, Culture invader)
    {
        Culture culture = cultureTurnInfo.Culture;
        // ability to repel is function of population and affinity (and later tech)
        //TODO: re-add affinity information so that repel ability is function of new affinity
        float hasAffinityAdvantage = 0;
        float popAdvantage = ((float)culture.Population - invader.Population) / 10f;
        float repelThreshold = .6f + hasAffinityAdvantage + popAdvantage;
        //Debug.Log("repel threshold = .6 + " + hasAffinityAdvantage + " + " + popAdvantage);
        if (Random.value < repelThreshold)
        {
            Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, invader, Culture.State.Repelled));
            //Debug.Log(invader.name + " is repelled by " + culture.name);
            return true;
        }
        else
        {
            Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, invader, Culture.State.Default));

            //Debug.Log(invader.Tile.name);
            //EventManager.TriggerEvent("PauseSpeed", null);
            return false;
        }
=======
public class AttemptRepelAction : CultureTurnInfo
{
    public AttemptRepelAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        return AttemptRepel();
    }

    Turn AttemptRepel()
    {
        foreach (Culture c in Culture.tileInfo.orderToRemoveCulturesIn)
        {
            if (c.currentState == Culture.State.Invader)
            {
                // ability to repel is function of population and affinity (and later tech)
                //TODO: re-add affinity information so that repel ability is function of new affinity
                float hasAffinityAdvantage = 0;
                float popAdvantage = ((float)Culture.Population - c.Population) / 10f;
                float repelThreshold = .6f + hasAffinityAdvantage + popAdvantage;
                //Debug.Log("repel threshold = .6 + " + hasAffinityAdvantage + " + " + popAdvantage);
                if (Random.value < repelThreshold)
                {
                    Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, c, Culture.State.Repelled));
                    Debug.Log(c.name + " is repelled by " + Culture.name);
                }
                else
                {
                    Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, c, Culture.State.Default));

                    Debug.Log(c.Tile.name);
                    //EventManager.TriggerEvent("PauseSpeed", null);
                }
                if(Random.value < .01f)
                {
                    Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(this, Culture, -1)); // killed in repelling effort
                    //Debug.Log("some of " + Culture.name + " killed in repel");
                }
                break;
            }
        }
        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, Culture, Culture.State.Default));
        return turn;
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }
}
