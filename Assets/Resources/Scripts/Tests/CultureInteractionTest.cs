using System.Collections;
using UnityEngine;
using NUnit.Framework;

public class CultureInteractionTest : CultureActionTest
{
    protected GameObject NeighborObject;
    protected Culture Neighbor;

    [SetUp]
    public void CreateNeighbor()
    {
        NeighborObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Inhabitants/Culture"));
        Neighbor = NeighborObject.GetComponent<Culture>();
    }
    
}
