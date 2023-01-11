using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardStats : MonoBehaviour
{
    public int equator
    {
        get
        {
            return isHemisphere ? 0 : Mathf.FloorToInt(Height / 2);
        }
    }
    public int maxDistFromEquator
    {
        get
        {
            return isHemisphere ? Height : Mathf.CeilToInt(Height / 2f);
        }
    }

    BoardEdges _boardEdges;
    public BoardEdges BoardEdges { get { return _boardEdges; } }

    public event EventHandler<OnBoardDimensionsChangedEventArgs> OnBoardDimensionsChanged;

    public float seaLevel
    {
        get
        {
            return NormalizedSeaLevel * elevationRange;
        }
    }

    public void Initialize(float tileWidth)
    {
        TileWidth = tileWidth;
        _boardEdges = new BoardEdges(Height, Width, TileWidth);
    }

    public void SetDimensions(int w, int h)
    {
        _height = h;
        _width = w;
        ResetBoardDimensions();
    }

    public void SetTileWidth(float tw)
    {
        TileWidth = tw;
        _boardEdges = new BoardEdges(Height, Width, TileWidth);
        ResetBoardDimensions();
    }

    void ResetBoardDimensions()
    {
        Debug.Log("setting dimensions");
        _boardEdges = new BoardEdges(Height, Width, TileWidth);
        OnBoardDimensionsChanged?.Invoke(this, new OnBoardDimensionsChangedEventArgs() { BoardStats = this, BoardEdges = _boardEdges });
    }

    public int Age; // age in days since the start of the board

    [SerializeField]
    int _height;
    [SerializeField]
    int _width;

    public int Height { get { return _height; } }
    public int Width { get { return _width; } }

    public float TileWidth { get; private set; }

    public float tempVariance = 25f;

    public float LandRisePoint;
    public float NormalizedSeaLevel;
    public float globalTemp;
    public float globalPrecipitation;
    public bool isHemisphere = false;
 

    public float elevationRange;


    public GameObject[] tileTypes;


    public class OnBoardDimensionsChangedEventArgs : EventArgs
    {
        public BoardEdges BoardEdges;
        public BoardStats BoardStats;
    }


}
