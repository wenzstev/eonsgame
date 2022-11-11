using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class CurveTestSuite
{


    [Test]
    public void TestGaussianCurve()
    {
        GaussianCurve TestCurve = new GaussianCurve(3, 10, 3);
        Assert.AreEqual(3, TestUtils.ThreeDecimals(TestCurve.GetPointOnCurve(10)), "MaxHeight is not as expected!");
        Assert.AreEqual(.748f, TestUtils.ThreeDecimals(TestCurve.GetPointOnCurve(5)), "Value at 5 is not as expected!");
        Assert.AreEqual(.012f, TestUtils.ThreeDecimals(TestCurve.GetPointOnCurve(20)), "Point at 20 is not as expected!");
    }

    [Test]
    public void TestSigmoidCurve()
    {
        SigmoidCurve TestCurve = new SigmoidCurve(5, 2, -3);
        Assert.AreEqual(.012f, TestUtils.ThreeDecimals(TestCurve.GetPointOnCurve(0)), "Value at 0 is not as expected!");
        Assert.AreEqual(4.685f, TestUtils.ThreeDecimals(TestCurve.GetPointOnCurve(2.9f)), "Value at 2.9 is not as expected!");
    }

    [Test]
    public void TestPowerCurve()
    {
        PowerCurve TestCurve = new PowerCurve(1.3f, 8);
        Assert.AreEqual(.455f, TestUtils.ThreeDecimals(TestCurve.GetPointOnCurve(5)), "Value at 5 is not as expected!");
        Assert.AreEqual(39.374f, TestUtils.ThreeDecimals(TestCurve.GetPointOnCurve(22)), "Value at 22 was not as expected!");
    }

    [Test]
    public void TestCompoundCurve()
    {
        GaussianCurve TestGaussianCurve = new GaussianCurve(2, 24, 10);
        PowerCurve TestPowerCurve = new PowerCurve(-2f, 43);
        CompoundCurve TestCurve = new CompoundCurve(new List<ICurve> { TestGaussianCurve, TestPowerCurve });

        Assert.AreEqual(.751f, TestUtils.ThreeDecimals(TestCurve.GetPointOnCurve(10)), "Value at 10 is not as expected!");
        Assert.AreEqual(2, TestUtils.ThreeDecimals(TestCurve.GetPointOnCurve(24)), "Value at 24 is not as expected!");
        Assert.AreEqual(-7.822f, TestUtils.ThreeDecimals(TestCurve.GetPointOnCurve(46)), "Value at 46 is not as expected!");

    }
}