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
        TestBoardStats.SetDimensions(2, 1);

        TestBoardStats.LandRisePoint = 0;

        TestBoard.CreateBoard();
    }
}