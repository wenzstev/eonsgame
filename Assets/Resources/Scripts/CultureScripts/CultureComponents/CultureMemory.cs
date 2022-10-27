using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CultureMemory : MonoBehaviour
{
    [SerializeField]
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

    [SerializeField]
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




}
