using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class BoardStatsTestSuite 
{
    BoardStats TestBoardStats;

    [SetUp]
    public void SetUpBoardStatsTest()
    {
        GameObject BoardTestObj = new GameObject("BoardStatsTest");
        TestBoardStats = BoardTestObj.AddComponent<BoardStats>();
    }

    [Test]
    public void CanGetAccurateBoardEdges()
    {
        TestBoardStats.SetDimensions(5, 5);
        TestBoardStats.SetTileWidth(1);


        Vector2[] Edges = new Vector2[]
        {
            new Vector2(-.5f, -.5f),
            new Vector2(-.5f, 4.5f),
            new Vector2(4.5f, 4.5f),
            new Vector2(4.5f, -.5f)
        };

        Assert.AreEqual(Edges[0], TestBoardStats.BoardEdges.BottomLeft);
        Assert.AreEqual(Edges[1], TestBoardStats.BoardEdges.TopLeft);
        Assert.AreEqual(Edges[2], TestBoardStats.BoardEdges.TopRight);
        Assert.AreEqual(Edges[3], TestBoardStats.BoardEdges.BottomRight);
    }

}
