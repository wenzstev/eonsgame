using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public abstract class CultureActionTest : BasicBoardTest
{
    protected GameObject TestCultureObj;
    protected Culture TestCulture;
    protected GameObject TestTileObj;
    protected Tile TestTile;
    protected GameObject NeighborTileObj;
    protected Tile NeighborTile;

    [UnitySetUp]
    public IEnumerator SetupCultureActionTest()
    {
        TestCultureObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Inhabitants/Culture"));
        TestCulture = TestCultureObj.GetComponent<Culture>();

        TestTileObj = TestBoard.GetTile(0, 0);
        TestTile = TestTileObj.GetComponent<Tile>();

        NeighborTileObj = TestBoard.GetTile(1, 0);
        NeighborTile = NeighborTileObj.GetComponent<Tile>();

        TestCulture.Init(TestTile, 1);
        Debug.Log($"Setup Complete. Culture name is {TestCulture}");
        yield return null;
    }


    [TearDown]
    public void TearDownCultureTest()
    {
        Turn.UpdateAllCultures();
    }
}
