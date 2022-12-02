using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
public class CulturePlacementHandlerTestSuite : BasicTest
{
    Culture TestCultureA;
    Culture TestCultureB;
    Culture TestCultureC;

    CultureContainer TestCultureContainer;
    CulturePlacementHandler TestCulturePlacementHandler;

    Vector3 TestPosition;

   [SetUp]
    public void SetupCulturePlacementHandlerTest()
    {
        TestCultureA = CreateCultureObj("TestCultureA");
        TestCultureA.AddPopulation(10);
        TestCultureB = CreateCultureObj("TestCultureB");
        TestCultureB.AddPopulation(5);
        TestCultureC = CreateCultureObj("TestCultureC");
        TestCultureC.AddPopulation(3);

        GameObject TestCulturePlacementObj = new GameObject("TestCulturePlacementObj");
        TestPosition = new Vector3(50, 50, 0); // set to different position to make sure transform happens in world, not just local, space
        TestCulturePlacementObj.transform.position = TestPosition;
        TestCultureContainer = TestCulturePlacementObj.AddComponent<CultureContainer>();
        TestCulturePlacementHandler = TestCulturePlacementObj.AddComponent<CulturePlacementHandler>();
        TestCulturePlacementHandler.CultureContainer = TestCultureContainer;
    }

    [UnityTest]
    public IEnumerator CanShowOneCultureOnTile()
    {
        TestCultureContainer.AddCulture(TestCultureA);

        yield return new WaitForSeconds(TestCulturePlacementHandler.AnimationTransferTime);

        Assert.AreEqual(Vector3.zero + TestPosition, TestCultureA.transform.position);
    }

    [UnityTest]
    public IEnumerator CanShowTwoCulturesOnTile()
    {
        TestCultureContainer.AddCulture(TestCultureA);
        TestCultureContainer.AddCulture(TestCultureB);

        yield return new WaitForSeconds(TestCulturePlacementHandler.AnimationTransferTime);

        Assert.AreEqual(new Vector3(-1, 0) + TestPosition, TestCultureA.transform.position);
        Assert.AreEqual(new Vector3(1, 0) + TestPosition, TestCultureB.transform.position);
    }

    [UnityTest]
    public IEnumerator CanShowThreeCulturesOnTile()
    {
        TestCultureContainer.AddCulture(TestCultureA);
        TestCultureContainer.AddCulture(TestCultureB);
        TestCultureContainer.AddCulture(TestCultureC);

        yield return new WaitForSeconds(TestCulturePlacementHandler.AnimationTransferTime);

        Assert.AreEqual(new Vector3(0, 1) + TestPosition, TestCultureA.transform.position);
        Assert.AreEqual(new Vector3(-Mathf.Sqrt(3f) / 2f, -.5f) + TestPosition, TestCultureB.transform.position);
        Assert.AreEqual(new Vector3(Mathf.Sqrt(3f) / 2f, -.5f) + TestPosition, TestCultureC.transform.position);

    }

    [UnityTest]
    public IEnumerator CanReduceFromThreeToTwoCulturesOnTile()
    {
        TestCultureContainer.AddCulture(TestCultureA);
        TestCultureContainer.AddCulture(TestCultureB);
        TestCultureContainer.AddCulture(TestCultureC);

        yield return new WaitForSeconds(TestCulturePlacementHandler.AnimationTransferTime);

        TestCultureContainer.RemoveCulture(TestCultureA);

        yield return new WaitForSeconds(TestCulturePlacementHandler.AnimationTransferTime);

        Assert.AreEqual(new Vector3(-1, 0) + TestPosition, TestCultureB.transform.position);
        Assert.AreEqual(new Vector3(1, 0) + TestPosition, TestCultureC.transform.position);
    }

    [UnityTest]
    public IEnumerator CanNotShowAdditionalCulturesAfterThree()
    {
        GameObject TestCultureDObj = new GameObject("TestCultureD");
        Culture TestCultureD = TestCultureDObj.AddComponent<Culture>();

        TestCultureContainer.AddCulture(TestCultureA);
        TestCultureContainer.AddCulture(TestCultureB);
        TestCultureContainer.AddCulture(TestCultureC);
        TestCultureContainer.AddCulture(TestCultureD);

        yield return new WaitForSeconds(TestCulturePlacementHandler.AnimationTransferTime);

        Assert.AreEqual(new Vector3(Mathf.Sqrt(3f) / 2f, -.5f) + TestPosition, TestCultureD.transform.position);
    }

    [UnityTest]
    public IEnumerator CanRearrangeWhenPopChanges()
    {
        TestCultureContainer.AddCulture(TestCultureA);
        TestCultureContainer.AddCulture(TestCultureB);
        TestCultureContainer.AddCulture(TestCultureC);

        yield return new WaitForSeconds(TestCulturePlacementHandler.AnimationTransferTime);

        TestCultureC.AddPopulation(100);

        yield return new WaitForSeconds(TestCulturePlacementHandler.AnimationTransferTime);

        Assert.AreEqual(new Vector3(0, 1) + TestPosition, TestCultureC.transform.position);
        Assert.AreEqual(new Vector3(-Mathf.Sqrt(3f) / 2f, -.5f) + TestPosition, TestCultureA.transform.position);
        Assert.AreEqual(new Vector3(Mathf.Sqrt(3f) / 2f, -.5f) + TestPosition, TestCultureB.transform.position);
    }

    [TearDown]
    public void TearDownCulturePlacementHandlerTest()
    {
        TestUtils.TearDownTest();
    }

    Culture CreateCultureObj(string name)
    {
        GameObject CultureObj = new GameObject(name);
        return CultureObj.AddComponent<Culture>();
    }

   
}
