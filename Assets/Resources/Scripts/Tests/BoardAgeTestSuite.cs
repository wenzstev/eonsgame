using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NUnit.Framework;
using UnityEngine.TestTools;

// TODO: longer term this could be turned into a UI test suite
public class BoardAgeTestSuite : BasicTest
{
    GameObject CanvasObj;
    TextMeshProUGUI TestText;

    GameObject TestBoardObj;
    BoardStats TestBoardStats;

    [UnitySetUp]
    public IEnumerator SetUpCanvas()
    {
        CanvasObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/MainScreen/Canvas"));
        TestText = CanvasObj.GetComponentInChildren<TextMeshProUGUI>();
        TestBoardObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Board"));
        TestBoardStats = TestBoardObj.GetComponent<BoardStats>();
        TestBoardStats.Age = 1000;

        yield return null;
    }

    [UnityTest]
    public IEnumerator CanGetBoardAgeAtStartup()
    {
        GetBoardAgeAtStartup();
        yield return null;
        Assert.AreEqual("2 / 9 / 10", TestText.text, "Board text has the incorrect date!");
    }

    [UnityTest]
    public IEnumerator CanUpdateBoardAge()
    {   
        GetBoardAgeAtStartup();
        yield return null;
        EventManager.TriggerEvent("Tick", null);
        yield return null;
        Assert.That(TestText.text == "2 / 9 / 11", "Board text did not incrememnt the date!");
    }

    IEnumerator GetBoardAgeAtStartup()
    {
        int counter = 0;
        while (counter < 50 && TestText.text == "")
        {
            yield return null;
        }
        Assert.That(counter <= 50, "Board Text Never got the date!");

    }
}
