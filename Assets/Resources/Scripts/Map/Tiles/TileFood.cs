using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFood : MonoBehaviour
{
    public float CurFood;
    public int MaxFood {
        get {
            return Mathf.FloorToInt(FoodModifier * NewFoodPerTick * 1000f);
        }
    }
    public float NewFoodPerTick;

    TileChars tileChars;

    public float FoodModifier;

    CompoundCurve TempCurve;
    CompoundCurve HumidityCurve;


    // Start is called before the first frame update
    void Start()
    {
        tileChars = GetComponent<TileChars>();
        TempCurve = CreateTempCurve();
        HumidityCurve = CreateHumidityCurve();
        CalculateFoodRate();
        SetMaxFood();
        EventManager.StartListening("Tick", OnTick);
    }


    public float CalculateFoodRate()
    {
 //       Debug.Log($"Temp: {TempCurve.GetPointOnCurve(tileChars.temperature) - .1f} Humidity: {HumidityCurve.GetPointOnCurve(tileChars.humidity)}");
        float climateModifier = TempCurve.GetPointOnCurve(tileChars.temperature) - .1f + HumidityCurve.GetPointOnCurve(tileChars.humidity);
        NewFoodPerTick = climateModifier * FoodModifier;
        return NewFoodPerTick;
    }

    CompoundCurve CreateTempCurve()
    {
        GaussianCurve gaussComponent = new GaussianCurve(.5f, 24, 10);
        PowerCurve powComponent = new PowerCurve(-2f, 43);
        return new CompoundCurve(new List<ICurve> { gaussComponent, powComponent });
    }

    CompoundCurve CreateHumidityCurve()
    {
        SigmoidCurve sigmoidComponent = new SigmoidCurve(1, 100, -.04f);
        PowerCurve powComponent = new PowerCurve(-.5f, 15);
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
    }
}