
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public abstract class BasicTest 
{
    protected EventManager eventManager;

    [UnitySetUp]
    public IEnumerator SetUpBasicTest()
    {
        eventManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/EventManager")).GetComponent<EventManager>();
        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        TestUtils.TearDownTest();
    }
}
