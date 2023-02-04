using System.Collections;
using UnityEngine;
using System;

[Serializable]
public struct DecayTracker
{
    [SerializeField]
    int _daysSinceHarvested;

    [SerializeField]
    float _currentDecayRate;

    [SerializeField]
    PowerCurve _forgettingCurve;

    [SerializeField]
    float _totalDaysHarvested;

    public int DaysSinceHarvested { get { return _daysSinceHarvested; } }

    public float CurrentDecayRate { get { return _currentDecayRate; } } // the rate at which knowledge is lost

    public float TotalDaysHarvested { get { return _totalDaysHarvested; } }

    public DecayTracker(float NumDaysHarvested)
    {
        _daysSinceHarvested = 0;
        _currentDecayRate = -.5f;
        _forgettingCurve = new PowerCurve(2, 0, 100, -1 / NumDaysHarvested);
        _totalDaysHarvested = NumDaysHarvested;
        //Debug.Log($"forgetting curve for {biome} is {_forgettingCurve.PowerMultiplier}");
    }

    DecayTracker(float NumDaysHarvested, int DaysSinceHarvested)
    {
        _daysSinceHarvested = DaysSinceHarvested;
        _currentDecayRate = -.5f;
        _forgettingCurve = new PowerCurve(2, 0, 100, -1 / NumDaysHarvested);
        _totalDaysHarvested = NumDaysHarvested;
    }
    

    public float GetDecayRate()
    {
        //Debug.Log($"Days since harvested: {DaysSinceHarvested}");
        //Debug.Log($"Decaying affinity by { _forgettingCurve.GetPointOnCurve(DaysSinceHarvested) * .01f}");
        return _forgettingCurve.GetPointOnCurve(DaysSinceHarvested) * .01f;
    }

    public void IncreaseDaysSinceHarvested() // TODO: some sort of easy way to get the current date so that this is unnecessary
    {
        _daysSinceHarvested += 1;
    }

    public static DecayTracker CombineDecayRates(DecayTracker first, DecayTracker second, float ratio)
    {
        second._daysSinceHarvested = Mathf.FloorToInt(Mathf.Lerp(first.DaysSinceHarvested, second.DaysSinceHarvested, ratio));
        second._totalDaysHarvested = Mathf.FloorToInt(Mathf.Lerp(first.TotalDaysHarvested, second.TotalDaysHarvested, ratio));
        second._currentDecayRate = .5f;

        return second;
    }
}