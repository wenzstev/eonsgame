using System.Collections;
using UnityEngine;
using NUnit.Framework;
using System.Linq;

public class TurnTestSuite : BasicTest
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

        Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 0));
        Assert.AreEqual(1, Turn.CurrentTurn.UpdateHolder.GetIntUpdates().Length);
    }

    [Test]
    
    public void CanGetPendingUpdatesFor()
    {
        GameObject OtherCultureObj = new GameObject("OtherCulture");
        Culture OtherCulture = OtherCultureObj.AddComponent<Culture>();
        Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, OtherCulture, 0));
        Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 0));
        Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 0));
        Assert.AreEqual(2, Turn.CurrentTurn.UpdateHolder.GetIntUpdates().Where(u => u.Target == TestCulture).ToArray().Length);
    }

    [Test]
    public void CanUpdateAllCultures()
    {
        Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 1));
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
