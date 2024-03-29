using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Profiling;

[Serializable]
public class AffinityManager : MonoBehaviour
{
    [SerializeField]
    TileDrawer.BiomeType _currentBiome;

    [SerializeField]
    public GompertzCurve AffinityGrowthRate;

    [SerializeField]
    AffinityStats affinityStats;

    public float Asymptote = -1f;
    public float XDisplacement = 2;
    public float GrowthRate = .3f;
    public float YDisplacement = 1f;

    public event EventHandler<OnAffinityChangedEventArgs> OnAffinityChanged;


    public AffinityStats AffinityStats { get { return affinityStats; } }

    private void Awake()
    {
        EventManager.StartListening("Tick", OnTick);
    }

    public void Initialize()
    {
        AffinityGrowthRate = new GompertzCurve(Asymptote, XDisplacement, GrowthRate, YDisplacement);
        _currentBiome = TileDrawer.BiomeType.Barren;
        affinityStats = AffinityStats.InitializeStats();
    }

    public void Initialize(AffinityManager parent)
    {
        AffinityGrowthRate = new GompertzCurve(Asymptote, XDisplacement, GrowthRate, YDisplacement);
        affinityStats = parent.GetStatCopy();
        _currentBiome = TileDrawer.BiomeType.Barren;
    }

    public void SetStats(AffinityStats stats)
    {
        affinityStats = stats;
    }

    public float GetAffinity(TileDrawer.BiomeType biome)
    {
        return affinityStats.GetAffinity(biome);
    }


    public void ChangeAffinity(TileDrawer.BiomeType biome, float newAmount)
    {
        affinityStats.ChangeAffinity(biome, newAmount);
        OnAffinityChanged?.Invoke(this, new OnAffinityChangedEventArgs() { Biome = biome });
    }

    public void HarvestedOnBiome(TileDrawer.BiomeType biome)
    {
        affinityStats.HarvestOnBiome(biome);
        //Debug.Log($"Days harvested for biome is {daysHarvested[biome]}");

        if(_currentBiome != biome) 
        {
            // create decay curve for current affinity
            //Debug.Log($"Setting decay rate for biome {biome} for culture: {GetComponent<Culture>()}");
            AdjustNewAffinityForDecay(biome);
        }
        _currentBiome = biome;
        IncrementAffinity(biome);
    }

    void AdjustNewAffinityForDecay(TileDrawer.BiomeType newBiome)
    {
        affinityStats.AddNewDecayTracker(_currentBiome);
        affinityStats.UpdateAffinityForDecay(newBiome);
        OnAffinityChanged?.Invoke(this, new OnAffinityChangedEventArgs() { Biome = newBiome });
    }

    public void IncrementAffinity(TileDrawer.BiomeType biome)
    {
        float currentAffinity = affinityStats.GetAffinity(biome);
        float valToAdd = AffinityGrowthRate.GetPointOnCurve(currentAffinity);
        ChangeAffinity(biome, currentAffinity + valToAdd);
    }

    public (TileDrawer.BiomeType, float)[] GetAllAffinities()
    {
        return affinityStats.GetAllAffinities();
    }

    public void OnTick(Dictionary<string, object> empty)
    {
        if(gameObject.activeInHierarchy) affinityStats.IncrementDecayDays(_currentBiome);
    }


    public AffinityStats GetStatCopy()
    {
        return affinityStats; // does this auto copy the struct?
    }

    public AffinityStats GetStatMerge(AffinityManager other, float ratio)
    {
        return affinityStats.CombineAffinities(other.AffinityStats, ratio);
    }

    public class OnAffinityChangedEventArgs : EventArgs
    {
        public TileDrawer.BiomeType Biome;
    }
}
