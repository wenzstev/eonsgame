using System.Collections;
using UnityEngine;

/// <summary>
/// Get all needed componets of the tile at start and cache them
/// </summary>
public class TileComponents : MonoBehaviour
{
    Tile _tile;
    TileChars _tileChars;
    TileFood _tileFood;

    public Tile Tile { get { return _tile; } }
    public TileChars TileChars { get { return _tileChars; } }
    public TileFood TileFood { get { return _tileFood; } }

    private void Awake()
    {
        _tile = GetComponent<Tile>();
        _tileChars = GetComponent<TileChars>();
        _tileFood = GetComponent<TileFood>();
    }
}
