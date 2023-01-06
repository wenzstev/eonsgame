using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject TileInfoPanel;
    public GameObject CultureInfoPanel;
    public Canvas canvas;

    GameObject CurrentTile;
    GameObject CurrentCulture;

    GameObject CurrentTileInfoPanel;



    private void Awake()
    {
        EventManager.StartListening("MouseUpTile", OnMouseUpTile);
    }


    void OnMouseUpTile(Dictionary<string, object> NewFocusedTile)
    {
        SelectNewTile((GameObject)NewFocusedTile["tile"]);
    }

    void SelectNewTile(GameObject NewSelectedTile)
    {
        Destroy(CurrentTileInfoPanel);
        CurrentTile = NewSelectedTile;
        CurrentTileInfoPanel = CreateTileInfoPanel(CurrentTile);
    }

    GameObject CreateTileInfoPanel(GameObject SelectedTile)
    {
        GameObject NewTileInfoPanel = Instantiate(TileInfoPanel, canvas.transform);
        NewTileInfoPanel.GetComponent<TileInfoPanelController>().SetValues(SelectedTile);
        return NewTileInfoPanel;
    }
}
