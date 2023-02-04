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
        AffinityStats firstStats = AffinityStats.InitializeStats();
        AffinityStats secondStats = AffinityStats.InitializeStats();

        firstStats.ChangeAffinity(TileDrawer.BiomeType.Desert, 50);
        secondStats.ChangeAffinity(TileDrawer.BiomeType.Taiga, 25);

        AffinityStats CombinedStats = firstStats.CombineAffinities(secondStats, .5f);

        Assert.AreEqual(25f, CombinedStats.GetAffinity(TileDrawer.BiomeType.Desert));
        Assert.AreEqual(12.5f, CombinedStats.GetAffinity(TileDrawer.BiomeType.Taiga));
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
