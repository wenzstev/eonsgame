using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLoader : MonoBehaviour
{
    public GameObject TileTemplate;

    public void LoadTilesFromSerialized(GameObject board, SerializedTiles st)
    {
        int height = board.GetComponent<Board>().Height;
        int width = board.GetComponent<Board>().Width;

        GameObject[,] boardTiles = new GameObject[width, height];
        Dictionary<GameObject, (int, int)> tileLookup = new Dictionary<GameObject, (int, int)>();

        int i = 0;
        int j = 0;

        foreach (SerializedTile t in st.tiles)
        {
            //Debug.Log($"Deserizalizing {i}, {j} into {t.type}");
            GameObject curTile = createTile(t);
            boardTiles[i, j] = curTile;
            tileLookup.Add(curTile, (i, j));
            curTile.GetComponent<Tile>().id = j * width + 1;

            i++;
            if (i >= width)
            {
                i = 0;
                j++;
            }
        }

        board.GetComponent<Board>().tiles = new BoardTileRelationship(boardTiles, tileLookup);
    }


    public GameObject createTile(SerializedTile st)
    {
        GameObject newTile = Instantiate(TileTemplate, new Vector3(st.x, st.y, 0), Quaternion.identity);
        newTile.transform.SetParent(transform);
        newTile.GetComponent<Tile>().board = GetComponent<Board>();
        newTile.name = st.x + ", " + st.y;
        return newTile;
    }
}
