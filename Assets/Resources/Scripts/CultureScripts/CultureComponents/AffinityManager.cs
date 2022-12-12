using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class AffinityManager : MonoBehaviour
{
    TileDrawer.BiomeType _currentBiome;

    Dictionary<TileDrawer.BiomeType, float> affinities;
    Dictionary<TileDrawer.BiomeType, DecayTracker> decayRates;
    Dictionary<TileDrawer.BiomeType, int> daysHarvested;
    GompertzCurve AffinityGrowthRate;

    public float Asymptote = -1f;
    public float XDisplacement = 2;
    public float GrowthRate = .5f;
    public float YDisplacement = 1f;
    


    public void Initialize()
    {
        affinities = new Dictionary<TileDrawer.BiomeType, float>();
        decayRates = new Dictionary<TileDrawer.BiomeType, DecayTracker>();
        daysHarvested = new Dictionary<TileDrawer.BiomeType, int>();
        foreach (TileDrawer.BiomeType biome in Enum.GetValues(typeof(TileDrawer.BiomeType)))
        {
            affinities.Add(biome, 0); // cultures are bad at gathering food initially
            daysHarvested.Add(biome, 0);
        }
        AffinityGrowthRate = new GompertzCurve(Asymptote, XDisplacement, GrowthRate, YDisplacement);
        _currentBiome = TileDrawer.BiomeType.Barren;
        EventManager.StartListening("Tick", OnTick);
    }

    public float GetAffinity(TileDrawer.BiomeType biome)
    {
        return affinities[biome];
    }


    public void ChangeAffinity(TileDrawer.BiomeType biome, float newAmount)
    {
        affinities[biome] = newAmount;
    }

    public void HarvestedOnBiome(TileDrawer.BiomeType biome)
    {
        daysHarvested[biome] += 1;
        //Debug.Log($"Days harvested for biome is {daysHarvested[biome]}");

        if(_currentBiome != biome) 
        {
            // create decay curve for current affinity
            AdjustNewAffinityForDecay(biome);

        }
        _currentBiome = biome;
        IncrementAffinity(biome);
    }

    void AdjustNewAffinityForDecay(TileDrawer.BiomeType biome)
    {
            DecayTracker newDecayTracker = new DecayTracker(_currentBiome, daysHarvested[_currentBiome]);
            decayRates.Add(_currentBiome, newDecayTracker);

            if(decayRates.ContainsKey(biome))
            {
                float decayPercent = decayRates[biome].GetDecayRate();
                //Debug.Log($"decay rate for {biome} is {decayPercent}");
                ChangeAffinity(biome, affinities[biome] * decayPercent);
                decayRates.Remove(biome);
            }
    }

    public void IncrementAffinity(TileDrawer.BiomeType biome)
    {
        float currentAffinity = GetAffinity(biome);
        float valToAdd = AffinityGrowthRate.GetPointOnCurve(currentAffinity);
        ChangeAffinity(biome, currentAffinity + valToAdd);
    }

    public void OnTick(Dictionary<string, object> empty)
    {
        foreach(KeyValuePair<TileDrawer.BiomeType, DecayTracker> kvp in decayRates)
        {
            if(kvp.Key != _currentBiome) kvp.Value.IncreaseDaysSinceHarvested(); // all biomes that aren't the current one decay a bit
        }
    }


    class DecayTracker
    {
        public TileDrawer.BiomeType Biome { get; private set; }
        public int DaysSinceHarvested { get; private set;}
        PowerCurve _forgettingCurve;
        public float CurrentDecayRate { get; private set; } // the rate at which knowledge is lost

        public DecayTracker(TileDrawer.BiomeType biome, float NumDaysHarvested)
        {
            DaysSinceHarvested = 0;
            CurrentDecayRate = -.5f; 
            _forgettingCurve = new PowerCurve(2, 0, 100, -1/NumDaysHarvested);
            //Debug.Log($"forgetting curve for {biome} is {_forgettingCurve.PowerMultiplier}");
        }

        public float GetDecayRate()
        {
            return _forgettingCurve.GetPointOnCurve(DaysSinceHarvested) * .01f;
        }

        public void IncreaseDaysSinceHarvested() // TODO: some sort of easy way to get the current date so that this is unnecessary
        {
            DaysSinceHarvested += 1;
        }
    }


    /*

    maybe decay happens as a function of the amount of time since being on a tile?

    struct Decay
       Biome biome;
       int daysSinceHarvested;
       ExponentialCurve forgettingCurve
       float lastAffinityLevel

       memory decays exponentially; the curve is flattened the more that a culture has been on a tile
       don't calculate decay until a culture is ready to harvest from biome again





     void HarvestedOnBiome(biome b)
        use some curve to calculate the next 'step' up the affinity level
        figure out the increase in affinity and add it to the value
        decay all other affinities

    void DecayAffinity(biome b)
        all biomes that ARENT b,
            subtract certain amount based on curve


    keep track of accumulated days spent in each biome? adjust the forgetting curve accordingly?


     add value to existing? 

    
 

    ## forgetting curve
    3.038 -> 3.038 - .35
    2.688
     


    1. track current biome
    2. when harvesting:
        - on same biome -> increase affinity, add day to harvest day tracker
        - on new biome -> create decay tracker for previous biome, decay curve based on number of harvest days
            - check for existing decay curve on biome, return result as intial affinity level
            - destroy existing decay curve




     */

    // decay affinity? 



}
