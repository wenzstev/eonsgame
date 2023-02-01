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
    TileDrawer tileDrawer;

    /// <summary>
    /// Percent of max food that is replenished every tick
    /// </summary>
    [Range(0, .01f)]
    public float FoodGrowthPercent;

    SigmoidCurve PrecipitationCurve;


    public event EventHandler<OnFoodChangeEventArgs> OnFoodChange;

    private void Awake()
    {
        tileChars = GetComponent<TileChars>();
        tileDrawer = GetComponent<TileDrawer>();
        tileChars.OnAllTileStatsCalculated += TileFood_OnAllTileStatsCalculated;
    }

    public void DetermineMaxFood()
    {
        MaxFood = (int) (CalculateFoodRate() * MAX_FOOD_MODIFIER);
    }

    public void Initialize()
    {
        PrecipitationCurve = CreatePrecipitationCurve();
        DetermineMaxFood();


        NewFoodPerTick = CalculateFoodPerTick();
        EventManager.StartListening("Tick", OnTick);
        FireFoodAction();

    }

    float CalculateFoodRate()
    {
        if (tileChars.temperature > tileDrawer.tempMaxValue) return 0;
        if (tileChars.temperature < tileDrawer.tempMinValue) return 0;

        return PrecipitationCurve.GetPointOnCurve(tileChars.precipitation);
    }

    public float CalculateFoodPerTick()
    {
        return MaxFood * FoodGrowthPercent; 
    }


    SigmoidCurve CreatePrecipitationCurve()
    {
        return new SigmoidCurve(100, 100, -.05f);
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

    public struct OnFoodChangeEventArgs 
    {
        public float MaxFood;
        public float CurFood;
    }
}