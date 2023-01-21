using System.Collections;
using UnityEngine;
using NUnit.Framework;

public class TurnTestSuite 
{
    Culture TestCulture;
    CultureTurnInfo dummyTurnInfo;

    [SetUp]
    public void SetUpTurnTest()
    {
        GameObject TestCultureObj = new GameObject("testCulture");
        TestCulture = TestCultureObj.AddComponent<Culture>();
        dummyTurnInfo = new CultureTurnInfo();
    }

    [Test]
    public void TestAddUpdate()
    {

        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 0));
        INonGenericCultureUpdate[] updates = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(1, updates.Length);
    }

    [Test]
    
    public void CanGetPendingUpdatesFor()
    {
        GameObject OtherCultureObj = new GameObject("OtherCulture");
        Culture OtherCulture = OtherCultureObj.AddComponent<Culture>();
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, OtherCulture, 0));
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 0));
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 0));
        INonGenericCultureUpdate[] updates = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(2, updates.Length);
    }

    [Test]
    public void CanUpdateAllCultures()
    {
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 1));
        Turn.UpdateAllCultures();
        Assert.AreEqual(1, TestCulture.Population);
    }

    [Test]
    public void CanChangeTurnAfterUpdated()
    {
        Turn prevTurn = Turn.CurrentTurn;
        Turn.UpdateAllCultures();
        Assert.AreNotEqual(prevTurn, Turn.CurrentTurn);
    }

    [TearDown]
    public void OnTurnTestTeardown()
    {
        Turn.UpdateAllCultures();
    }

}
