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
<<<<<<< HEAD
        previousTile = cm.previousTile.TileLocation.id;
=======
        previousTile = cm.previousTile.id;
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        previousState = (int)cm.previousState;
        cultureParentName = cm.cultureParentName;
        wasRepelled = cm.wasRepelled;
    }
}