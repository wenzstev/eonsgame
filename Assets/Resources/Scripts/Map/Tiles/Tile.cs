using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public Board board;

    public int id;

    static GameObject _moveTile;
    public static GameObject moveTile
    {
        get
        {
            if(_moveTile == null)
            {
                _moveTile = Instantiate(Resources.Load<GameObject>("Prefabs/Board/Tile"));
                _moveTile.name = "Move Tile";
                return _moveTile;
            }
            return _moveTile;
        }
    }

    public Dictionary<Direction, GameObject> neighbors;

    void Start()
    {
        neighbors = new Dictionary<Direction, GameObject>();
    }

    public GameObject GetNeighbor(Direction d)
    {
        GameObject neighbor = null;
        if(neighbors.TryGetValue(d, out neighbor))
        {
            return neighbor;
        }
        else
        {
            neighbors[d] = board.getNeighbor(gameObject, d);

            return neighbors[d];
        }
    }

    public GameObject GetRandomNeighbor()
    {
        Direction randDir = (Direction) Mathf.FloorToInt(Random.value * 8);
        return GetNeighbor(randDir);
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