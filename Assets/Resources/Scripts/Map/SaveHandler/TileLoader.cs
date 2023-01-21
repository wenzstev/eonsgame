using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLoader : MonoBehaviour
{
    public GameObject TileTemplate;

    GameObject curBoard;

    public void LoadTilesFromSerialized(GameObject board, SerializedTiles st)
    {
        curBoard = board;
        int height = board.GetComponent<Board>().Height;
        int width = board.GetComponent<Board>().Width;

        Debug.Log($"{height} {width}");

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

            curTile.GetComponent<TileLocation>().id = j * width + i;

            i++;
            if (i >= width)
            {
                i = 0;
                j++;
            }
        }

        board.GetComponent<Board>().boardTileRelationship = new BoardTileRelationship(boardTiles, tileLookup);
    }


    public GameObject createTile(SerializedTile st)
    {
        GameObject newTile = Instantiate(TileTemplate);

        newTile.GetComponent<TileLocation>().board = curBoard.GetComponent<Board>();


        JsonUtility.FromJsonOverwrite(st.serializedComponents[0], newTile.GetComponent<TileChars>()); // presently hardcoding the index, inelegant but makes using FromJsonOverwrite very easy. TODO: better way?

        TileChars loadedTileChars = newTile.GetComponent<TileChars>();
        newTile.transform.position = new Vector3(loadedTileChars.x, loadedTileChars.y);
        newTile.GetComponent<TileDrawer>().Initialize();

        JsonUtility.FromJsonOverwrite(st.serializedComponents[1], newTile.GetComponent<TileFood>());
        newTile.GetComponentInChildren<TileGreyscaleOverlayLink>().Initialize();
        newTile.GetComponent<TileFood>().Initialize();

        newTile.transform.SetParent(curBoard.transform);
        newTile.name = loadedTileChars.x + ", " + loadedTileChars.y;

        newTile.GetComponentInChildren<CulturePlacementHandler>().Initialize();

        return newTile;
    }
}
