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
        Neighbor = CreateBarebonesCulture("Neighbor");
        NeighborObject = Neighbor.gameObject;
        yield return null;
    }
    
}
