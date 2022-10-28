using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public abstract class CultureActionTest
{
    protected EventManager eventManager;
    protected GameObject TestCultureObj;
    protected Culture TestCulture;
    protected GameObject TestTileObj;
    protected Tile TestTile;

    [SetUp]
    public void SetupCultureActionTest()
    {
        eventManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/EventManager")).GetComponent<EventManager>(); 
        TestCultureObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Inhabitants/Culture"));
        TestCulture = TestCultureObj.GetComponent<Culture>();

        TestTileObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/EmptyTile"));
        TestTile = TestTileObj.GetComponent<Tile>();

        TestCulture.Init(TestTile);
        Debug.Log($"Setup Complete. Culture name is {TestCulture}");
    }
    
    [TearDown]
    public void TearDownCultureTest()
    {
        Turn.HookTurn().UpdateAllCultures();
        TestUtils.TearDownTest();
    }
}
