using System.Collections;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class CultureInteractionTest : CultureActionTest
{
    protected GameObject NeighborObject;
    protected Culture Neighbor;

    [UnitySetUp]
    public IEnumerator CreateNeighbor()
    {
        NeighborObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Inhabitants/Culture"));
        Neighbor = NeighborObject.GetComponent<Culture>();
        yield return null;
    }
    
}
