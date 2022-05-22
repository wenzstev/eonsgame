using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultureAction
{
    protected Culture culture;
    public Turn turn;

    protected CultureAction(Culture c)
    {
        culture = c;
        turn = new Turn(c);
    }

    public abstract Turn ExecuteTurn();

}

public class DoNothingAction : CultureAction
{
    public DoNothingAction(Culture c) : base(c) { }
    public override Turn ExecuteTurn()
    {
        return turn;
    }
}

public class DefaultAction : CultureAction
{
    public DefaultAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {

        if (Random.value < culture.spreadChance)
        {
            CultureAction moveTile = new MoveTileAction(culture);
            return moveTile.ExecuteTurn();
        }

        turn.newState = culture.currentState;
        if (Random.value < culture.growPopulationChance && culture.population <= culture.maxOnTile)
        {
            turn.popChange++;
        }

        if (Random.value < culture.gainAffinityChance)
        {
            turn.newAffinity = culture.tileInfo.tileType;
        }

        return turn;


    }
}

public class DecisionMaker
{
    Culture culture;

    public DecisionMaker(Culture c)
    {
        culture = c;
    }

    public Turn ExecuteTurn()
    {
        CultureAction action = new DoNothingAction(culture);


        switch (culture.currentState)
        {
            case Culture.State.Default:
                action = new DefaultAction(culture);
                break;
            case Culture.State.Invaded:
                action = new AttemptRepelAction(culture);
                break;
            case Culture.State.Repelled:
                action = new RepelledAction(culture);
                break;
            default:
                break;
        }

        return action.ExecuteTurn();
    }
}

public class Turn
{
    public Culture culture;
    public Culture.State newState;
    public int popChange = 0;
    public int techChange = 0;
    public string newAffinity = "";
    public Color newColor;

    public Turn(Culture c)
    {
        culture = c;
        newState = c.currentState;
        newColor = c.mutateColor(c.color); // mutate color slightly every turn
    }


    public void pushChangesToCulture(Culture c)
    {
        c.AddPopulation(popChange);
        c.SetColor(newColor);
        c.currentState = newState;
        if(newAffinity != "")
        {
            c.GainAffinity(newAffinity);
        }

        EventManager.TriggerEvent("CultureUpdated", new Dictionary<string, object> { { "culture", c } });

    }

}
