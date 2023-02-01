using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine;

public class PerlinHeightmapGeneratorTestSuite
{
    PerlinHeightmapGenerator TestGenerator;
    GameObject TestGeneratorObj;

    [UnitySetUp]
    public IEnumerator SetUpPerlinBoard()
    {
        TestGeneratorObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/BoardGenerator"));
        TestGenerator = TestGeneratorObj.GetComponent<PerlinHeightmapGenerator>();
        yield return null;
    }


    [Test]
    public void CanAssignExpectedValues()
    {

        TestGenerator.LandRisePoint = .2f;
        TestGenerator.WaterLevel = .1f;
        TestGenerator.AvgAboveWaterElevation = .1f;
        TestGenerator.SeaLevelRiseSteepness = 15;

        CompoundCurve TestCompoundCurve = TestGenerator.HeightModifier;

        float[] testPoints = { 0f, .1f, .2f, .3f, .5f, .8f , .95f};
        float[] expectedValues = { 0.009f,0.036f,0.1f,0.1635f,0.1978f,0.2012f, .4146f};

        for(int i = 0; i < testPoints.Length; i++)
        {
            Assert.AreEqual(TestUtils.ThreeDecimals(expectedValues[i]), TestUtils.ThreeDecimals(TestCompoundCurve.GetPointOnCurve(testPoints[i])), "Point is not at expected value!");
        }

        


    }



}