using System;
using UnityEngine;
using NUnit.Framework;


public class CameraBoundsTestSuite 
{
    Camera TestCamera;
    CameraMoveController TestCameraMoveController;
    CameraBounds TestCameraBounds;
    BoardStats TestBoardStats;

    [SetUp]
    public void SetUpCameraBoundsTests()
    {
        GameObject TestCameraObject = new GameObject("Camera");
        GameObject DummyBoard = new GameObject("Dummy Board");

        TestCamera = TestCameraObject.AddComponent<Camera>();
        TestCamera.orthographic = true;
        TestCamera.aspect = 16f / 10f;
        TestCameraMoveController = TestCameraObject.AddComponent<CameraMoveController>();
        TestCameraBounds = TestCameraObject.AddComponent<CameraBounds>();
        TestCameraBounds.BufferZone = 4f;
        TestCameraBounds.CameraMoveController = TestCameraMoveController;
       

        TestBoardStats = DummyBoard.AddComponent<BoardStats>();
        TestBoardStats.SetDimensions(10, 7);
        TestBoardStats.SetTileWidth(1);

        MockBoardEdgesGetter mockBoardEdgesGetter = new MockBoardEdgesGetter(TestBoardStats.BoardEdges);

        TestCameraBounds.SetBoardEdgesGetter(mockBoardEdgesGetter);
    }

    [Test]
    public void CanGetBoundsFromBoard()
    {
        Assert.AreEqual(TestBoardStats.BoardEdges, TestCameraBounds.BoardEdges);
    }

    [Test]
    public void CanPreventCameraFromExceedingBoundsLarge()
    {
        TestCameraMoveController.AttemptMove(new CameraMovement(new Rect(new Vector2(0, 0), new Vector2(100, 100)), TestCamera));
        Assert.AreEqual(new Vector3(76, -43, 0), TestCamera.transform.position, "Camera did not properly stay in bounds!");
    }

    [Test]
    public void CanAllowCameraWhenLegalBoundsChange()
    {
        TestCameraMoveController.AttemptMove(new CameraMovement(new Rect(Vector2.zero, new Vector2(5, 5)), TestCamera));
        Assert.AreEqual(new Vector3(4f, 2.5f, 0), TestCamera.transform.position, "Camera was prevented from executing legal move!");
    }

    [Test]
    public void CanPreventCameraFromExceedingBoundsSmall()
    {
        TestCameraMoveController.AttemptMove(new CameraMovement(new Rect(new Vector2(-100, 3), new Vector2(5, 5)), TestCamera));
        Assert.AreEqual(new Vector3(0f, 4.5f, 0), TestCamera.transform.position, "Camera did not properly stay in bounds!");
    }


}

class MockBoardEdgesGetter : IBoardEdgesGetter
{
    BoardEdges boardEdges;
    public BoardEdges BoardEdges { get { return boardEdges; } }
    public EventHandler<EventArgs> OnBoardEdgesSet { get; set; }

    public MockBoardEdgesGetter(BoardEdges be)
    {
        boardEdges = be;
    }
}