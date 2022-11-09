using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class BasicBoardTest : BasicTest
{
    protected GameObject TestBoardObj;
    protected Board TestBoard;

    [UnitySetUp]
    public IEnumerator SetupBasicBoard()
    {
        SetupBoard();
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
}