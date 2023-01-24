
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public abstract class BasicTest 
{
    protected EventManager eventManager;
    protected CulturePool culturePool;

    [UnitySetUp]
    public IEnumerator SetUpBasicTest()
    {
        eventManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/EventManager")).GetComponent<EventManager>();
        GameObject CulturePoolObject = new GameObject("Culture Pool");
        CulturePool culturePool = CulturePoolObject.AddComponent<CulturePool>();
        culturePool._culturePrefab = Resources.Load<GameObject>("Prefabs/Board/Inhabitants/Culture").GetComponent<Culture>();
        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        TestUtils.TearDownTest();
    }
}
