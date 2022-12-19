using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;



public class CameraMovementTestSuite 
{
    Camera TestCamera;
    CameraMovement TestCameraMovement;
    GameObject TestCameraObject;

    [SetUp]
    public void SetUpCameraMovementTest()
    {
        TestCameraObject = new GameObject("Camera");
        TestCamera = TestCameraObject.AddComponent<Camera>();
        TestCamera.aspect = (16f / 10f);
        TestCamera.orthographic = true;

    }

    [Test]
    public void CanDetermineNewTransformFromBounds()
    {
        TestCameraMovement = GenerateTransform(new Rect(0, 0, 50, 50));

        Assert.AreEqual(new Vector3(40, 25, 0), TestCameraMovement.NewOrigin);
        Assert.AreEqual(25, TestCameraMovement.NewZoom);
    }

    [Test]
    public void NewTransformIsAccurate()
    {
        TestCameraMovement = GenerateTransform(new Rect(0, 0, 50, 50));
        TestCameraMovement.ExecuteMove();

        Vector2 bottomLeft = TestCamera.ViewportToWorldPoint(Vector3.zero);
        Vector2 topRight = TestCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Assert.AreEqual(Vector2.zero, bottomLeft);
        Assert.AreEqual(new Vector2(80, 50), new Vector2(Mathf.RoundToInt(topRight.x), Mathf.Round(topRight.y))); // camera edges aren't precise
    }

    public CameraMovement GenerateTransform(Rect rect)
    {
        return new CameraMovement(rect, TestCamera);
    }
}
