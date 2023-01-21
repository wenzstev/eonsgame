using System.Collections;
using UnityEngine;

[System.Serializable]
public class SerializedCultureMemory
{
    public int previousTile;
    public int previousState;
    public string cultureParentName;
    public bool wasRepelled;

    public SerializedCultureMemory(CultureMemory cm)
    {
        previousTile = cm.previousTile.TileLocation.id;
        previousState = (int)cm.previousState;
        cultureParentName = cm.cultureParentName;
        wasRepelled = cm.wasRepelled;
    }
}