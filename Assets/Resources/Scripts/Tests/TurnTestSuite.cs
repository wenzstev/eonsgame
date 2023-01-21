using System.Collections;
using UnityEngine;
using NUnit.Framework;

public class TurnTestSuite 
{
    Culture TestCulture;
<<<<<<< HEAD
    CultureTurnInfo dummyTurnInfo;
=======
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074

    [SetUp]
    public void SetUpTurnTest()
    {
        GameObject TestCultureObj = new GameObject("testCulture");
        TestCulture = TestCultureObj.AddComponent<Culture>();
<<<<<<< HEAD
        dummyTurnInfo = new CultureTurnInfo();
=======
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }

    [Test]
    public void TestAddUpdate()
    {

<<<<<<< HEAD
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 0));
=======
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(null, TestCulture, 0));
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        INonGenericCultureUpdate[] updates = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(1, updates.Length);
    }

    [Test]
    
    public void CanGetPendingUpdatesFor()
    {
        GameObject OtherCultureObj = new GameObject("OtherCulture");
        Culture OtherCulture = OtherCultureObj.AddComponent<Culture>();
<<<<<<< HEAD
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, OtherCulture, 0));
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 0));
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 0));
=======
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(null, OtherCulture, 0));
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(null, TestCulture, 0));
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(null, TestCulture, 0));
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        INonGenericCultureUpdate[] updates = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(2, updates.Length);
    }

    [Test]
    public void CanUpdateAllCultures()
    {
<<<<<<< HEAD
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(dummyTurnInfo, TestCulture, 1));
=======
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(null, TestCulture, 1));
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
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
