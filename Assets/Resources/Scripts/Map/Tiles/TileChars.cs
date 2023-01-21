using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileChars : MonoBehaviour
{
    public int x;
    public int y;

    public event EventHandler<OnAllTileStatsCalculatedEventArgs> OnAllTileStatsCalculated;


    public TileDrawer.BiomeType Biome
    {
        get
        {
            return GetComponent<TileDrawer>().tileType; // should change, this is too tightly coupled to tiledrawer
        }
    }

    public float elevation
    {
        get
        {
            return Mathf.Max(0, absoluteHeight - boardStats.seaLevel);
        }
    }

    public bool isUnderwater
    {
        get
        {
            return elevation == 0;
        }
    }

    public bool isFrozenOver
    {
        get
        {
            float threshold = isUnderwater ? -5 : 0;
            return temperature < threshold;
        }
    }


    public bool isCoast;
    public float absoluteHeight;
    public float precipitation;
    public float temperature;

    BoardStats _boardStats;
    public BoardStats boardStats
    {
        get
        {
            if(_boardStats == null)
            {
<<<<<<< HEAD
                _boardStats = GetComponent<TileLocation>().board.GetComponent<BoardStats>();
=======
                _boardStats = GetComponent<Tile>().board.GetComponent<BoardStats>();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
            }
            return _boardStats;
        }
    }

    public void InformAllStatsCalculated()
    {
        OnAllTileStatsCalculated?.Invoke(this, new OnAllTileStatsCalculatedEventArgs() { TileChars = this });
    }

    public class OnAllTileStatsCalculatedEventArgs : EventArgs
    {
        public TileChars TileChars;
    }
}
