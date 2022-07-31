using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureMemory : MonoBehaviour
{

    Tile _previousTile;
    public Tile previousTile { 
        get => _previousTile;
        set 
        {
            if(value != null && value.gameObject != Tile.moveTile)
            {
                _previousTile = value;
            }
        } 
    }

    public string cultureParentName;

    Culture.State _previousState;

    public Culture.State previousState
    {
        get => _previousState;
        set
        {
            if(value == Culture.State.Repelled)
            {
                wasRepelled = true;
            }
            _previousState = value;
        }
    }

    public bool wasRepelled;

    public void LoadFromSave(SerializedCultureMemory scm, Tile currentTile)
    {
        previousTile = currentTile.board.GetComponent<Board>().GetTileByID(scm.previousTile).GetComponent<Tile>(); // whoof
        cultureParentName = scm.cultureParentName;
        _previousState = (Culture.State)scm.previousState;
        wasRepelled = scm.wasRepelled;
    }


}
