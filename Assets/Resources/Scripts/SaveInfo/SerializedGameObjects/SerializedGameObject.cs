using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SerializedGameObject
{

    public List<string> serializedComponents;

    public SerializedGameObject()
    {
        serializedComponents = new List<string>();
    }

}
