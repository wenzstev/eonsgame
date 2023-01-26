using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using System.Text;

[Serializable]
public struct AffinityStats 
{
    [SerializeField]
    float[] affinities;

    [SerializeField]
    DecayTracker[] decayRates;

    [SerializeField]
    int[] daysHarvested;

    public static AffinityStats InitializeStats()
    {
        AffinityStats newStats = new AffinityStats();
        int numBiomes = Enum.GetNames(typeof(TileDrawer.BiomeType)).Length;

        newStats.affinities = new float[numBiomes];
        newStats.decayRates = Enumerable.Range(0, numBiomes).Select(e => new DecayTracker(0)).ToArray();
        newStats.daysHarvested = new int[numBiomes];

        return newStats;
    }


    AffinityStats (float[] a, DecayTracker[] dr, int[] dh)
    {
        affinities = a;
        decayRates = dr;
        daysHarvested = dh;
    }

    public float GetAffinity(TileDrawer.BiomeType biome)
    {
        return affinities[(int) biome];
    }

    public void ChangeAffinity(TileDrawer.BiomeType biome, float newAmount)
    {
        affinities[(int)biome] = newAmount;
    }

    public void HarvestOnBiome(TileDrawer.BiomeType biome)
    {
        daysHarvested[(int)biome] += 1;
    }

    public int GetNumDaysHarvested(TileDrawer.BiomeType biome)
    {
        return daysHarvested[(int)biome];
    }

    public void AddNewDecayTracker(TileDrawer.BiomeType biome)
    {
        int numDaysHarvested = GetNumDaysHarvested(biome);
        decayRates[(int)biome] = new DecayTracker(numDaysHarvested);
    }

    public void UpdateAffinityForDecay(TileDrawer.BiomeType biome)
    {
        if (decayRates[(int)biome].TotalDaysHarvested != 0)
        {
            float decayPercent = decayRates[(int)biome].GetDecayRate();
            //Debug.Log($"decay rate for {biome} is {decayPercent}");
            ChangeAffinity(biome, GetAffinity(biome) * decayPercent);
            decayRates[(int)biome] = new DecayTracker(0);
        }
    }

    public void IncrementDecayDays(TileDrawer.BiomeType current)
    {
        for (int i = 0; i < decayRates.Length; i++)
        {
            if(i != (int) current) decayRates[i].IncreaseDaysSinceHarvested();
        }
    }

    public (TileDrawer.BiomeType, float)[] GetAllAffinities()
    {
        return affinities.Select((a, i) => ((TileDrawer.BiomeType) i, a)).ToArray();
    }

    public AffinityStats CreateCopy()
    {
        return this;      
    }

    public float GetDecayRate(TileDrawer.BiomeType biome)
    {
        try
        {
            return decayRates[(int)biome].GetDecayRate();
        }
        catch (NullReferenceException e) // this is thrown if a decay rate is requested for a biome a culture has never been on
        {
            Debug.LogWarning("Issue with getting decay rate: " + e.Message);
            return 0;
        }
    }

    public DecayTracker GetDecayTracker(TileDrawer.BiomeType biome)
    {
        return decayRates[(int)biome];
    }

    public AffinityStats CombineAffinities(AffinityStats other, float ratio)
    {
        for(int i = 0; i < other.affinities.Length; i++)
        {
            other.affinities[i] = Mathf.Lerp(affinities[i], other.affinities[i], ratio);
            other.daysHarvested[i] = Mathf.FloorToInt(Mathf.Lerp(daysHarvested[i], other.daysHarvested[i], ratio));
            other.decayRates[i] = DecayTracker.CombineDecayRates(decayRates[i], other.decayRates[i], ratio);
        }


        return other; // since it's a struct, returning other doesn't change struct that was brought in
    }



}
