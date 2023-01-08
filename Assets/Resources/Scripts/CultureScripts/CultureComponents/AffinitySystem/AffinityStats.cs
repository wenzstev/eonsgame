using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using System.Text;

[Serializable]
public class AffinityStats 
{
    [SerializeField]
    float[] affinities;

    [SerializeField]
    DecayTracker[] decayRates;

    [SerializeField]
    int[] daysHarvested;

    public AffinityStats()
    {
        int numBiomes = Enum.GetNames(typeof(TileDrawer.BiomeType)).Length;

        affinities = new float[numBiomes];
        decayRates = Enumerable.Range(0, numBiomes).Select(e => new DecayTracker(0)).ToArray();
        daysHarvested = new int[numBiomes];
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
            if(i != (int) current) decayRates[i]?.IncreaseDaysSinceHarvested();
        }
    }

    public (TileDrawer.BiomeType, float)[] GetAllAffinities()
    {
        return affinities.Select((a, i) => ((TileDrawer.BiomeType) i, a)).ToArray();
    }

    public AffinityStats CreateCopy()
    {
        string affinitySerialization = JsonUtility.ToJson(this);
        AffinityStats copy = JsonUtility.FromJson<AffinityStats>(affinitySerialization);
        return copy;        
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
        int numBiomes = Enum.GetNames(typeof(TileDrawer.BiomeType)).Length;
        float[] combinedAffinities = new float[numBiomes];
        int[] combinedDays = new int[numBiomes];
        DecayTracker[] combinedDecayRates = new DecayTracker[numBiomes];

        foreach(TileDrawer.BiomeType biome in Enum.GetValues(typeof(TileDrawer.BiomeType)))
        {
            int curIndex = (int)biome;
            combinedAffinities[curIndex] = Mathf.Lerp(affinities[curIndex], other.GetAffinity(biome), ratio);
            combinedDays[curIndex] = Mathf.FloorToInt(Mathf.Lerp(daysHarvested[curIndex], other.GetNumDaysHarvested(biome), ratio));
            combinedDecayRates[curIndex] = DecayTracker.CombineDecayRates(decayRates[(int)biome], other.GetDecayTracker(biome), ratio);
        }

        return new AffinityStats(combinedAffinities, combinedDecayRates, combinedDays);

    }



}
