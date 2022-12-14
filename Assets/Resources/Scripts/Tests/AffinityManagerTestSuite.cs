using System.Collections;
using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Linq;


public class AffinityManagerTestSuite : BasicTest
{
    GameObject TestAffinityObj;
    AffinityManager TestAffinityManager;

    [UnitySetUp]
    public IEnumerator SetUpAffinityTests()
    {
        TestAffinityObj = new GameObject("TestAffinity");
        TestAffinityManager = TestAffinityObj.AddComponent<AffinityManager>();
        TestAffinityManager.Asymptote = -1f;
        TestAffinityManager.XDisplacement = 2;
        TestAffinityManager.GrowthRate = .5f;
        TestAffinityManager.YDisplacement = 1f;
        TestAffinityManager.Initialize();
        yield return null;
    }


    [Test]
    public void CanGainAffinityOnBiome()
    {
        HarvestForDays(5, TileDrawer.BiomeType.Grassland);
        Assert.AreEqual(TestUtils.ThreeDecimals(3.08303f), TestUtils.ThreeDecimals(TestAffinityManager.GetAffinity(TileDrawer.BiomeType.Grassland)), "Affinity did not gain at expected rate!");
        foreach (TileDrawer.BiomeType biome in Enum.GetValues(typeof(TileDrawer.BiomeType)))
        {
            if(biome != TileDrawer.BiomeType.Grassland) Assert.That(TestAffinityManager.GetAffinity(biome) == 0, $"Affinity should be 0, but {biome} is at {TestAffinityManager.GetAffinity(biome)}!");
        }
    }

    [Test]
    public void CanDecayAffinityOnBiome()
    {
        HarvestForDays(30, TileDrawer.BiomeType.Grassland);
        HarvestForDays(1, TileDrawer.BiomeType.Savannah);
        HarvestForDays(7, TileDrawer.BiomeType.Grassland);

        Assert.AreEqual(TestUtils.ThreeDecimals(7.018f), TestUtils.ThreeDecimals(TestAffinityManager.GetAffinity(TileDrawer.BiomeType.Grassland)), "Affinity did not decay at expected rate!");
    }

    [Test]
    public void CanGetClonedAffinityManager()
    {
        HarvestForDays(30, TileDrawer.BiomeType.Grassland);
        HarvestForDays(1, TileDrawer.BiomeType.Savannah);
        HarvestForDays(7, TileDrawer.BiomeType.Grassland);

        AffinityStats newStats = TestAffinityManager.GetStatCopy();
        foreach (TileDrawer.BiomeType biome in Enum.GetValues(typeof(TileDrawer.BiomeType)))
        {
            Debug.Log($"Testing {biome}");
            Assert.AreEqual(TestAffinityManager.AffinityStats.GetAffinity(biome), newStats.GetAffinity(biome), $"{biome} does not have expected affinity!");
            Assert.AreEqual(TestAffinityManager.AffinityStats.GetNumDaysHarvested(biome), newStats.GetNumDaysHarvested(biome), $"{biome} does not have expected days harvested!");
            Assert.AreEqual(TestAffinityManager.AffinityStats.GetDecayRate(biome), newStats.GetDecayRate(biome), $"{biome} does not have expected decay rate!");
        }
    }


    void HarvestForDays(int numDays, TileDrawer.BiomeType biome)
    {
        foreach(var _ in Enumerable.Range(0, numDays))
        {
            TestAffinityManager.HarvestedOnBiome(biome);
            EventManager.TriggerEvent("Tick", null);
            //Debug.Log("Harvesting. Affinity is now " + TestAffinityManager.GetAffinity(biome));
        }
        Debug.Log($"Final affinity for {biome} is {TestAffinityManager.GetAffinity(biome)}");
    }


    


}
