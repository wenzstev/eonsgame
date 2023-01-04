using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class MoveActionTestSuite : CultureActionTest
{
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        NeighborTile.GetComponent<TileDrawer>().tileType = TileDrawer.BiomeType.Grassland; // otherwise the minimum height will always be underwater

        yield return null;
    }


    [UnityTest]
    public IEnumerator MoveTileActionTest()
    {

        MoveTileAction mta = new MoveTileAction(TestCulture);
        mta.moveChance = 1;
        Turn TestTurn = mta.ExecuteTurn();
        yield return null;

        Turn.UpdateAllCultures();

        Assert.AreEqual(Culture.State.Moving, TestCulture.currentState, "Culture is not moving!");

        yield return new WaitForSeconds(.03f);

        Assert.That(TestCultureObj.transform.position != TestTile.transform.position);

        yield return new WaitForSeconds(.1f);

        Turn.UpdateAllCultures();

        Assert.AreEqual(Culture.State.NewOnTile, TestCulture.currentState);
        Assert.AreEqual(TestCulture, NeighborTile.GetComponentInChildren<CultureHandler>().GetAllStagedCultures()[0]);
        Assert.That(!TestTile.GetComponentInChildren<CultureHandler>().HasCultureByName(TestCulture.Name), "Previous tile still has the culture!");
    }

    [UnityTest]
    public IEnumerator MoveTileActionWhenLargeTest()
    {
        GameObject TimeControllerObj = new GameObject("Time Controller");
        TimeControllerObj.AddComponent<TimeController>();
        TimeController.instance.speeds = new float[] { 1 };

        TestCulture.AddPopulation(20);

        MoveTileAction mta = new MoveTileAction(TestCulture);
        mta.moveChance = 1;
        Turn testTurn = mta.ExecuteTurn();

        yield return null;

        Turn.UpdateAllCultures();

        Assert.That(TestCulture.Population <= TestCulture.maxPopTransfer && TestCulture.Population >= TestCulture.minPopTransfer, $"Expected pop to be in range {TestCulture.minPopTransfer}, {TestCulture.maxPopTransfer} but pop is actually {TestCulture.Population}!");

        yield return new WaitForSeconds(.1f);

        Turn.UpdateAllCultures();

        Assert.That(NeighborTile.GetComponentInChildren<CultureStaging>().transform.childCount > 0, "Culture staging does not have children!");

        GameObject childCultureObj = NeighborTile.GetComponentInChildren<CultureStaging>().transform.GetChild(0).gameObject;

        Culture childCulture = childCultureObj.GetComponent<Culture>();

        Assert.That(childCulture != null, "Tile child is not a culture object!");

        Assert.That(childCulture.Population < childCulture.maxPopTransfer && childCulture.Population > childCulture.minPopTransfer, $"Expected pop to be in range {childCulture.minPopTransfer}, {childCulture.maxPopTransfer} but pop is actually {childCulture.Population}!");
        Assert.That(childCulture.currentState == Culture.State.NewOnTile, "Child's state is not NewOnTile!");



    }


    [UnityTest]
    public IEnumerator RepelledActionTest()
    {
        TestCulture.GetComponent<CultureMemory>().previousTile = NeighborTile.GetComponent<Tile>();

        RepelledAction tra = new RepelledAction(TestCulture);
        tra.ExecuteTurn();
        Turn.UpdateAllCultures();

        yield return null;

        Assert.That(TestCulture.currentState == Culture.State.Moving, "Culture did not change to moving!");

        yield return new WaitForSeconds(.1f);

        Turn.UpdateAllCultures();

        Assert.That(TestCulture.currentState == Culture.State.NewOnTile, "Culture is not in NewOnTile State!");
    }
}