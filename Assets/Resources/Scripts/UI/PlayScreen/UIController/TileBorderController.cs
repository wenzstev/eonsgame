using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileBorderController : MonoBehaviour
{
    public UIController UIController;
    public GameObject TileHighlightTemplate;

    GameObject CurrentHighlight;
    GameObject CurrentTile;




    private void Awake()
    {
        UIController.OnTileSelected += UIController_OnTileSelected;
    }

    public void DeselectTile()
    {
        Destroy(CurrentHighlight);
        CurrentTile = null;
    }


    void HighlightTile(GameObject NewHighlightedTile)
    {
        DeselectTile();
        CurrentTile = NewHighlightedTile;
        CurrentHighlight = Instantiate(TileHighlightTemplate, CurrentTile.transform);
    }

    void UIController_OnTileSelected(object sender, UIController.OnTileSelectedArgs e)
    {
        HighlightTile(e.SelectedTile);
        e.SelectedTileInfoPanel.GetComponent<TileInfoPanelController>().OnPanelDestroyed += TileInfoPanelController_OnPanelDestroyed;
        
    }

    void TileInfoPanelController_OnPanelDestroyed(object sender, EventArgs e)
    {
        DeselectTile();
    }

   



}
