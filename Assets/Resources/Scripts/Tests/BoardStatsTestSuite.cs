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
<<<<<<< HEAD
            new Vector2(-.5f, -.5f),
            new Vector2(-.5f, 4.5f),
            new Vector2(4.5f, 4.5f),
            new Vector2(4.5f, -.5f)
=======
            Vector2.zero,
            new Vector2(0, 5),
            new Vector2(5, 5),
            new Vector2(5, 0)
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        };

        Assert.AreEqual(Edges[0], TestBoardStats.BoardEdges.BottomLeft);
        Assert.AreEqual(Edges[1], TestBoardStats.BoardEdges.TopLeft);
        Assert.AreEqual(Edges[2], TestBoardStats.BoardEdges.TopRight);
        Assert.AreEqual(Edges[3], TestBoardStats.BoardEdges.BottomRight);
    }

}
