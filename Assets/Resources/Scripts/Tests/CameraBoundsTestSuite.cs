using System.Collections;
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
        TestCameraMoveController = TestCameraObject.AddComponent<CameraMoveController>();
        TestCameraBounds = TestCameraObject.AddComponent<CameraBounds>();
        TestCameraBounds.BufferZone = 4f;
        TestCameraBounds.CameraMoveController = TestCameraMoveController;
       

        TestBoardStats = DummyBoard.AddComponent<BoardStats>();
        TestBoardStats.SetDimensions(10, 7);
        TestBoardStats.SetTileWidth(1);

        TestCameraBounds.SetBoard(TestBoardStats);
    }

    [Test]
    public void CanGetBoundsFromBoard()
    {
        Assert.AreEqual(TestBoardStats.BoardEdges, TestCameraBounds.BoardEdges);
    }

    [Test]
    public void CanPreventCameraFromExceedingBounds()
    {
        TestCameraMoveController.AttemptMove(new CameraMovement(new Rect(new Vector2(100, 0), new Vector2(100, 100)), TestCamera));
        Assert.AreEqual(new Vector3(10, 7, 0), TestCamera.transform.position, "Camera did not properly stay in bounds!");
    }

    [Test]
    public void CanResizeCameraBoundsWhenZooming()
    {
        TestCameraMoveController.AttemptMove(new CameraMovement(new Rect(new Vector2(100, 0), new Vector2(50, 50)), TestCamera));

        Assert.AreEqual(false, "Need to figure out where the camera would be!");
    }

}
