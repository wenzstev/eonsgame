using System.Collections;
using UnityEngine;
using System;
using NUnit.Framework;
using System.Linq;


public class AffinityStatsTestSuite
{
    AffinityStats TestAffinityStats;

    [SetUp]
    public void SetUpAffinityStatsTest()
    {
        TestAffinityStats = AffinityStats.InitializeStats();
    }


    [Test]
    public void CanDuplicateAffinity()
    {
        HarvestForDays(4, TileDrawer.BiomeType.Grassland, TestAffinityStats);
        HarvestForDays(3, TileDrawer.BiomeType.Desert, TestAffinityStats);
        HarvestForDays(30, TileDrawer.BiomeType.Savannah, TestAffinityStats);


        AffinityStats clone = TestAffinityStats.CreateCopy();
        foreach (TileDrawer.BiomeType biome in Enum.GetValues(typeof(TileDrawer.BiomeType)))
        {
            Debug.Log($"Testing {biome}");
            Assert.AreEqual(TestAffinityStats.GetAffinity(biome), clone.GetAffinity(biome), $"{biome} does not have expected affinity!");
            Assert.AreEqual(TestAffinityStats.GetNumDaysHarvested(biome), clone.GetNumDaysHarvested(biome), $"{biome} does not have expected days harvested!");
            Assert.AreEqual(TestAffinityStats.GetDecayRate(biome), clone.GetDecayRate(biome), $"{biome} does not have expected decay rate!");
        }
    }


    [Test]
    public void CanCombineAffinity()
    {
        AffinityStats SecondStats = AffinityStats.InitializeStats();

        HarvestForDays(4, TileDrawer.BiomeType.Grassland, TestAffinityStats);
        HarvestForDays(5, TileDrawer.BiomeType.TemperateRainforest, TestAffinityStats);

        HarvestForDays(3, TileDrawer.BiomeType.Grassland, SecondStats);
        HarvestForDays(7, TileDrawer.BiomeType.Desert, SecondStats);

        AffinityStats combinedStats = TestAffinityStats.CombineAffinities(SecondStats, .5f);
        int[] testDays = new int[] { 3, 0, 0, 3, 0, 0, 2, 0, 0, 0, 0, 0 };
        float[] testAffinities = new float[] { 35, 0, 0, 35, 0, 0, 25, 0, 0, 0, 0, 0 };
        float[] decayRates = new float[] { .396850228f, 0, 0, .25f, 0, 0, 0.176776692f, 0, 0, 0, 0, 0 };

        /*
         *         
         *         Desert,
        Savannah,
        TropicalRainforest,
        Grassland,
        Woodland,
        SeasonalForest,
        TemperateRainForest,
        BorealForest,
        Tundra,
        Ice,
        Water,
        Barren
         */

        foreach (TileDrawer.BiomeType biome in Enum.GetValues(typeof(TileDrawer.BiomeType)))
        {
            Assert.AreEqual(testAffinities[(int)biome], combinedStats.GetAffinity(biome), $"{biome} does not have expected affinity!");
            Assert.AreEqual(testDays[(int)biome], combinedStats.GetNumDaysHarvested(biome), $"{biome} does not have expected days harvested!");
            Assert.AreEqual(decayRates[(int)biome], combinedStats.GetDecayRate(biome), $"{biome} does not have expected decay rate!");
        }


    }


    public void HarvestForDays(int numDays, TileDrawer.BiomeType biome, AffinityStats affinityStats)
    {
        foreach (var _ in Enumerable.Range(0, numDays))
        {
            affinityStats.HarvestOnBiome(biome);
            affinityStats.IncrementDecayDays(biome);
            affinityStats.ChangeAffinity(biome, affinityStats.GetAffinity(biome) + 10);
            //Debug.Log("Harvesting. Affinity is now " + TestAffinityManager.GetAffinity(biome));
        }
        affinityStats.AddNewDecayTracker(biome);
        Debug.Log($"Final affinity for {biome} is {affinityStats.GetAffinity(biome)}");

    }

}
