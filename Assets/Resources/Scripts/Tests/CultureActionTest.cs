using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public abstract class CultureActionTest : BasicTest
{
    protected GameObject TestCultureObj;
    protected Culture TestCulture;
    protected GameObject TestTileObj;
    protected GameObject TestBoardObj;
    protected Board TestBoard;
    protected Tile TestTile;
    protected GameObject NeighborTileObj;
    protected Tile NeighborTile;

    [UnitySetUp]
    public IEnumerator SetupCultureActionTest()
    {

        SetupBoard();
        // create the mock board
        yield return null;


        TestCultureObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Inhabitants/Culture"));
        TestCulture = TestCultureObj.GetComponent<Culture>();

        TestTileObj = TestBoard.GetTile(0, 0);
        TestTile = TestTileObj.GetComponent<Tile>();

        NeighborTileObj = TestBoard.GetTile(1, 0);
        NeighborTile = NeighborTileObj.GetComponent<Tile>();

        TestCulture.Init(TestTile);
        Debug.Log($"Setup Complete. Culture name is {TestCulture}");
        yield return null;
    }

    protected void SetupBoard()
    {

        TestBoardObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Board"));
        GameObject TestBoardGenObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/BoardGenerator"));

        TestBoard = TestBoardObj.GetComponent<Board>();
        TestBoard.GetComponent<BoardInputReader>().bg = TestBoardGenObj.GetComponent<ImprovedBoardGen>();

        BoardStats TestBoardStats = TestBoard.GetComponent<BoardStats>();

        TestBoardStats.height = 1;
        TestBoardStats.width = 2;
        TestBoardStats.percentUnderWater = 0;

        TestBoard.CreateBoard();
        PrepMoveTileForBoard();

    }

    void PrepMoveTileForBoard()
    {
        // have to add a tiledrawer and set the type and board for the move action
        TileDrawer MoveTileDrawer = Tile.moveTile.AddComponent<TileDrawer>();
        MoveTileDrawer.tileType = TileDrawer.BiomeType.Grassland;
        Debug.Log($"setting test board: {TestBoard}");
        Tile.moveTile.GetComponent<Tile>().board = TestBoard;
        Debug.Log(Tile.moveTile.GetComponent<Tile>().board);
    }

    [TearDown]
    public void TearDownCultureTest()
    {
        Turn.HookTurn().UpdateAllCultures();
    }
}
