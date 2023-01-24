using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileChars : MonoBehaviour
{
    public int x;
    public int y;

    public event EventHandler<OnAllTileStatsCalculatedEventArgs> OnAllTileStatsCalculated;

    TileDrawer tileDrawer;

    public TileDrawer.BiomeType Biome { get { return tileDrawer.tileType; } }

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
                _boardStats = GetComponent<TileLocation>().board.GetComponent<BoardStats>();
            }
            return _boardStats;
        }
    }

    private void Awake()
    {
        tileDrawer = GetComponent<TileDrawer>();


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
