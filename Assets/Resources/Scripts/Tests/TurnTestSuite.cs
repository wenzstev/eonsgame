﻿using System.Collections;
using UnityEngine;
using NUnit.Framework;

public class TurnTestSuite 
{
    Culture TestCulture;

    [SetUp]
    public void SetUpTurnTest()
    {
        GameObject TestCultureObj = new GameObject("testCulture");
        TestCulture = TestCultureObj.AddComponent<Culture>();
    }

    [Test]
    public void TestAddUpdate()
    {

        Turn.AddUpdate(new MockTurnUpdate(TestCulture));
        INonGenericCultureUpdate[] updates = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(1, updates.Length);
    }

    [Test]
    
    public void CanGetPendingUpdatesFor()
    {
        GameObject OtherCultureObj = new GameObject("OtherCulture");
        Culture OtherCulture = OtherCultureObj.AddComponent<Culture>();
        Turn.AddUpdate(new MockTurnUpdate(OtherCulture));
        Turn.AddUpdate(new MockTurnUpdate(TestCulture));
        Turn.AddUpdate(new MockTurnUpdate(TestCulture));
        INonGenericCultureUpdate[] updates = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(2, updates.Length);
    }

    [Test]
    public void CanUpdateAllCultures()
    {
        Turn.AddUpdate(new PopulationUpdate(null, TestCulture, 1));
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

public class MockTurnUpdate : CultureUpdate<int>
{
    public override void ExecuteChange() { }
    public MockTurnUpdate(Culture c) : base(null, c, 0) { }
}
