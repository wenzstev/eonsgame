using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class AffinityManager : MonoBehaviour
{
    Dictionary<TileDrawer.BiomeType, float> affinities;
    GompertzCurve AffinityGrowthRate;
    ExponentialCurve AffinityDecayRate;

    public float Asymptote = -1f;
    public float XDisplacement = 2;
    public float GrowthRate = .5f;
    public float YDisplacement = 1f;

    public float DecaySteepness = .1f;


    public void Initialize()
    {
        affinities = new Dictionary<TileDrawer.BiomeType, float>();
        foreach (TileDrawer.BiomeType biome in Enum.GetValues(typeof(TileDrawer.BiomeType)))
        {
            affinities.Add(biome, 1f); // cultures are bad at gathering food initially
        }
        AffinityGrowthRate = new GompertzCurve(Asymptote, XDisplacement, GrowthRate, YDisplacement);
        AffinityDecayRate = new ExponentialCurve(DecaySteepness);
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
        float currentAffinity = GetAffinity(biome);
        float valToAdd = AffinityGrowthRate.GetPointOnCurve(currentAffinity);
        ChangeAffinity(biome, currentAffinity + valToAdd);
    }

    public void DecayAffinities(TileDrawer.BiomeType biome)
    {
        foreach(TileDrawer.BiomeType curBiome in Enum.GetValues(typeof(TileDrawer.BiomeType)))
        {
            if(curBiome != biome)
            {
                float currentAffinity = GetAffinity(curBiome);
                float valToSubtract = AffinityDecayRate.GetPointOnCurve(currentAffinity) -1;
                ChangeAffinity(curBiome, currentAffinity - valToSubtract);
            }
        }
    }

    /*
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
     
     */

    // decay affinity? 



}
