using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entry class for all tile information. Use this instead of GetComponent
/// </summary>
public class Tile : MonoBehaviour
{
    TileLocation _tileLocation ;
    TileChars _tileChars;
    TileFood _tileFood;
    TileDrawer _tileDrawer;

    public TileLocation TileLocation 
    { 
        get
        {
            if (_tileLocation == null) _tileLocation = GetComponent<TileLocation>();
            return _tileLocation;
        }
    }

    public TileChars TileChars
    {
        get
        {
            if (_tileChars == null) _tileChars = GetComponent<TileChars>();
            return _tileChars;
        }


    }

    public TileFood TileFood
    {
        get
        {
            if (_tileFood == null) _tileFood = GetComponent<TileFood>();
            return _tileFood;
        }
    }

    public TileDrawer TileDrawer
    {
        get
        {
            if (_tileDrawer == null) _tileDrawer = GetComponent<TileDrawer>();
            return _tileDrawer;
        }
    }
}


