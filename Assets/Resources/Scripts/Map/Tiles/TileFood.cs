using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class TileFood : MonoBehaviour
{
    public float CurFood;
    public float MAX_FOOD_MODIFIER = 1f;
    public int MaxFood { get; private set; }
    
    public float NewFoodPerTick;

    TileChars tileChars;

    /// <summary>
    /// Percent of max food that is replenished every tick
    /// </summary>
    [Range(0, .01f)]
    public float FoodGrowthPercent;

    CompoundCurve TempCurve;
    CompoundCurve PrecipitationCurve;


    public event EventHandler<OnFoodChangeEventArgs> OnFoodChange;

    private void Awake()
    {
        tileChars = GetComponent<TileChars>();
        tileChars.OnAllTileStatsCalculated += TileFood_OnAllTileStatsCalculated;
    }

    void DetermineMaxFood()
    {
        MaxFood = Mathf.Max(0, Mathf.FloorToInt(CalculateFoodRate() * MAX_FOOD_MODIFIER));
    }

    public void Initialize()
    {
        TempCurve = CreateTempCurve();
        PrecipitationCurve = CreatePrecipitationCurve();
        DetermineMaxFood();


        NewFoodPerTick = CalculateFoodPerTick();
        EventManager.StartListening("Tick", OnTick);
        FireFoodAction();

    }

    float CalculateFoodRate()
    {
        return TempCurve.GetPointOnCurve(tileChars.temperature) - .1f + PrecipitationCurve.GetPointOnCurve(tileChars.precipitation);
    }

    public float CalculateFoodPerTick()
    {
        return MaxFood * FoodGrowthPercent; 
    }

    CompoundCurve CreateTempCurve()
    {
        GaussianCurve gaussComponent = new GaussianCurve(.5f, 24, 10);
        PowerCurve powComponent = new PowerCurve(2f, 43, -1, 1);
        return new CompoundCurve(new List<ICurve> { gaussComponent, powComponent });
    }

    CompoundCurve CreatePrecipitationCurve()
    {
        SigmoidCurve sigmoidComponent = new SigmoidCurve(1, 100, -.04f);
        PowerCurve powComponent = new PowerCurve(.5f, 15, -1, 1);
        return new CompoundCurve(new List<ICurve> { sigmoidComponent, powComponent });
    }

    public void SetMaxFood()
    {
        CurFood = MaxFood;
    }

    public void OnTick(Dictionary<string, object> empty)
    {
        float newFood = CurFood + NewFoodPerTick;
        CurFood = newFood > MaxFood ? CurFood : newFood;
        FireFoodAction();
    }

    public void FireFoodAction()
    {
        OnFoodChange?.Invoke(this, new OnFoodChangeEventArgs {MaxFood = this.MaxFood, CurFood = this.CurFood });
    }

    public void TileFood_OnAllTileStatsCalculated(object sender, TileChars.OnAllTileStatsCalculatedEventArgs e)
    {
        Initialize();
        SetMaxFood();
    }

    public class OnFoodChangeEventArgs : EventArgs
    {
        public float MaxFood;
        public float CurFood;
    }
}