using System.Collections;
using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Linq;


public class CultureAffinityTestSuite 
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
        TestAffinityManager.DecaySteepness = .1f;
        TestAffinityManager.Initialize();
        yield return null;
    }

    // TODO: need to tweak the forgetting rate

    [Test]
    public void CanGainAffinityOnBiome()
    {
        HarvestForDays(100000);
        Assert.AreEqual(4.676436f, TestAffinityManager.GetAffinity(TileDrawer.BiomeType.Grassland), "Affinity did not gain at expected rate!");
        foreach (TileDrawer.BiomeType biome in Enum.GetValues(typeof(TileDrawer.BiomeType)))
        {
            if(biome != TileDrawer.BiomeType.Grassland) Assert.That(TestAffinityManager.GetAffinity(biome)! == 1, $"Affinity should be 1, but {biome} is at {TestAffinityManager.GetAffinity(biome)}!");
        }
    }

    [Test]
    public void CanDecayAffinityOnBiome()
    {
        HarvestForDays(1000);
        DecayForDays(100);
        Assert.AreEqual(0, TestAffinityManager.GetAffinity(TileDrawer.BiomeType.Grassland), "Affinity did not gain at expected rate!");

        foreach (TileDrawer.BiomeType biome in Enum.GetValues(typeof(TileDrawer.BiomeType)))
        {
            Assert.That(TestAffinityManager.GetAffinity(biome)! < 0, $"Affinity should never be less than zero, but {biome} is at {TestAffinityManager.GetAffinity(biome)}!");
        }
    
    }

 

    void HarvestForDays(int numDays)
    {
        foreach(var _ in Enumerable.Range(0, numDays))
        {
            //Debug.Log("Harvesting. Affinity is now " + TestAffinityManager.GetAffinity(TileDrawer.BiomeType.Grassland));
            TestAffinityManager.HarvestedOnBiome(TileDrawer.BiomeType.Grassland);
        }
    }

    void DecayForDays(int numDays)
    {
        foreach (var _ in Enumerable.Range(0, numDays))
        {
            Debug.Log("Decaying. Affinity is now " + TestAffinityManager.GetAffinity(TileDrawer.BiomeType.Grassland));
            TestAffinityManager.DecayAffinities(TileDrawer.BiomeType.Barren);
        }
    }

    


}
