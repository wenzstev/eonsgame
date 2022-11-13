using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{

    public Board board;

    public int id;

    static GameObject _moveTile;
    public static GameObject moveTile
    {
        get
        {
            if (_moveTile == null)
            {
                _moveTile = Instantiate(Resources.Load<GameObject>("Prefabs/Board/EmptyTile"));
                _moveTile.name = "Move Tile";
                Destroy(_moveTile.GetComponent<TileChars>());
                Destroy(_moveTile.GetComponent<TileDrawer>());
                 return _moveTile;
            }
            return _moveTile;
        }
    }

    Dictionary<Direction, GameObject> neighbors;
    List<Direction> neighborDirections;

    void Start()
    {
        neighbors = new Dictionary<Direction, GameObject>();
        neighborDirections = new List<Direction>();
        DetermineNeighbors();
    }

    void DetermineNeighbors()
    {
        Enumerable.Range(0, 8).Select((i) => GetNeighbor((Direction)i)).Count(); // count forces execution
    }


    public GameObject GetNeighbor(Direction d)
    {
        GameObject neighbor = null;
        if (neighbors.TryGetValue(d, out neighbor))
        {
            return neighbor;
        }
        else
        {
            neighbor = board.getNeighbor(gameObject, d);
            if (neighbor != null) AddNeighborTile(neighbor, d);

            return neighbor;
        }
    }

    void AddNeighborTile(GameObject neighbor, Direction d)
    {
        neighbors.Add(d, neighbor);
        neighborDirections.Add(d);
    }

    public GameObject GetRandomNeighbor()
    {
        int randDir = Mathf.FloorToInt(Random.value * neighborDirections.Count);
        Debug.Log($"Accessing list of count {neighborDirections.Count} at index {randDir}");
        return neighbors[neighborDirections[randDir]];

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