using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TileFood : MonoBehaviour
{
    public float CurFood;
    public float MAX_FOOD_MODIFIER = 1f;
    public int MaxFood {
        get {
            return Mathf.FloorToInt(FoodModifier * NewFoodPerTick * MAX_FOOD_MODIFIER);
        }
    }
    public float NewFoodPerTick;

    TileChars tileChars;

    public float FoodModifier;

    CompoundCurve TempCurve;
    CompoundCurve PrecipitationCurve;


    public event EventHandler<OnLowFoodEventArgs> OnLowFood;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        tileChars = GetComponent<TileChars>();
        TempCurve = CreateTempCurve();
        PrecipitationCurve = CreatePrecipitationCurve();
        CalculateFoodRate();
        SetMaxFood();
        EventManager.StartListening("Tick", OnTick);
    }

    public float CalculateFoodRate()
    {
        float climateModifier = TempCurve.GetPointOnCurve(tileChars.temperature) - .1f + PrecipitationCurve.GetPointOnCurve(tileChars.precipitation);
        NewFoodPerTick = Mathf.Max(0, climateModifier * FoodModifier);
        return NewFoodPerTick;
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
        OnLowFood?.Invoke(this, new OnLowFoodEventArgs {MaxFood = this.MaxFood, CurFood = this.CurFood });
    }

    public class OnLowFoodEventArgs : EventArgs
    {
        public float MaxFood;
        public float CurFood;
    }
}