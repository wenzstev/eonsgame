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



    
}
