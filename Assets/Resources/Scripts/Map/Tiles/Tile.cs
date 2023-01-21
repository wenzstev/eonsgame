using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
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
=======
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{

    public Board board;

    public int id;


    TileNeighbors _tileNeighbors;
    TileNeighbors NeighborTileGetter
    {
        get
        {
            if(_tileNeighbors == null)
            {
                _tileNeighbors = new TileNeighbors(this);
            }
            return _tileNeighbors;
        }
    }

    public GameObject GetNeighbor(Direction d)
    {
        return NeighborTileGetter.GetNeighbor(d);
    }

    public GameObject GetRandomNeighbor()
    {
        return NeighborTileGetter.GetRandomNeighbor();
    }

    public GameObject[] GetAllNeighbors()
    {
        return NeighborTileGetter.GetAllNeighbors();
    }

    class TileNeighbors
    {
        // the problem is that the DetermineNeighbors function needs to be called before the GetRandomNeighbor call,
        // but after the creation of the tile (because at that point the BoardTileRelationship that the function needs
        // to work). Maybe call this conditionally? It's not set up unless it needs to be set up, with some kind of 
        // getter/setter
        Dictionary<Direction, GameObject> neighbors;
        List<Direction> neighborDirections;
        Tile _tile;

        public TileNeighbors(Tile tile)
        {
            _tile = tile;
            DetermineNeighbors();
        }

        void DetermineNeighbors()
        {
            neighbors = new Dictionary<Direction, GameObject>();
            neighborDirections = new List<Direction>();
            Enumerable.Range(0, 8).Select((i) => GetNeighbor((Direction)i)).Count(); // count forces execution
        }

        public GameObject GetRandomNeighbor()
        {
            int randDir = Mathf.FloorToInt(Random.value * neighborDirections.Count);
            //Debug.Log($"Accessing list of count {neighborDirections.Count} at index {randDir}");
            return neighbors[neighborDirections[randDir]];
        }

        public GameObject GetNeighbor(Direction d)
        {
            if (_tile.board == null) return null; // for the movetile; it's kind of overstaying it's welcome

            GameObject neighbor = null;
            if (neighbors.TryGetValue(d, out neighbor))
            {
                return neighbor;
            }
            else
            {
                neighbor = _tile.board.getNeighbor(_tile.gameObject, d);
                if (neighbor != null) AddNeighborTile(neighbor, d);

                return neighbor;
            }
        }

        public GameObject[] GetAllNeighbors()
        {
            return neighbors.Select(kvp => kvp.Value).Where(go => go != null).ToArray();
        }

        void AddNeighborTile(GameObject neighbor, Direction d)
        {
            neighbors.Add(d, neighbor);
            neighborDirections.Add(d);
        }
    }
}   

public enum Direction
{
    N,
    NE,
    E,
    SE,
    S,
    SW,
    W,
    NW
}
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
