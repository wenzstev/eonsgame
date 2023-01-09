using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject TileInfoPanel;
    public GameObject CultureInfoPanel;
    public MouseActionsController MouseActionsController;
    public Canvas canvas;

    GameObject CurrentTile;
    GameObject CurrentCulture;

    GameObject CurrentTileInfoPanel;
    GameObject CurrentCultureInfoPanel;


    private void Awake()
    {
        MouseActionsController.MouseUpAction += UIController_OnMouseUpAction;
    }


    void OnMouseUpTile(Dictionary<string, object> NewFocusedTile)
    {
        SelectNewTile((GameObject)NewFocusedTile["tile"]);
    }

    void SelectNewTile(GameObject NewSelectedTile)
    {
        if(CurrentCultureInfoPanel)
        {
            StopListeningToOldTile();
        }

        Destroy(CurrentTileInfoPanel);
        CurrentTile = NewSelectedTile;
        CurrentTileInfoPanel = CreateTileInfoPanel(CurrentTile);
    }

    void SelectNewCulture(GameObject NewSelectedCulture)
    {
        Destroy(CurrentCultureInfoPanel);
        CurrentCulture = NewSelectedCulture;
        CurrentCultureInfoPanel = CreateCultureInfoPanel(NewSelectedCulture);

    }

    GameObject CreateTileInfoPanel(GameObject SelectedTile)
    {
        GameObject NewTileInfoPanel = Instantiate(TileInfoPanel, canvas.transform);
        NewTileInfoPanel.GetComponent<TileInfoPanelController>().SetValues(SelectedTile);

        CultureListButton[] cultures = NewTileInfoPanel.GetComponentsInChildren<CultureListButton>();
        foreach(CultureListButton button in cultures) button.OnCultureButtonClicked += CultureListButton_OnCultureButtonClicked;

        return NewTileInfoPanel;
    }

    private void CultureListButton_OnCultureButtonClicked(object sender, CultureListButton.OnCultureButtonClickedEventArgs e)
    {
        SelectNewCulture(e.Culture.gameObject);
    }

    GameObject CreateCultureInfoPanel(GameObject SelectedCulture)
    {
        GameObject NewCultureInfoPanel = Instantiate(CultureInfoPanel, canvas.transform);
        NewCultureInfoPanel.GetComponent<CulturePanelController>().SetValues(SelectedCulture);
        return NewCultureInfoPanel;
    }

    void StopListeningToOldTile()
    {
        CultureListButton[] cultures = CurrentCultureInfoPanel.GetComponentsInChildren<CultureListButton>();
        foreach (CultureListButton button in cultures) button.OnCultureButtonClicked -= CultureListButton_OnCultureButtonClicked;
    }

    void UIController_OnMouseUpAction(object sender, MouseActionsController.MouseActionEventArgs e)
    {
        GameObject[] clickedObjects = e.ClickedObjects;
        foreach (GameObject g in clickedObjects)
        {
            if(g.GetComponent<Tile>() != null)
            {
                SelectNewTile(g);
                continue;
            }
            if(g.GetComponent<Culture>() != null)
            {
                SelectNewCulture(g);
            }
            
        }
    }
}
