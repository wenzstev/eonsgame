using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject TileInfoPanel;
    public GameObject CultureInfoPanel;
    public MouseActionsController MouseActionsController;
    public Canvas canvas;

    public GameObject CurrentTile { get; private set; }
    GameObject CurrentCulture;

    GameObject CurrentTileInfoPanel;
    GameObject CurrentCultureInfoPanel;

    public event EventHandler<OnTileSelectedArgs> OnTileSelected;


    private void Awake()
    {
        MouseActionsController.MouseUpAction += UIController_OnMouseUpAction;
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
        OnTileSelected?.Invoke(this, new OnTileSelectedArgs() { SelectedTile = NewSelectedTile, SelectedTileInfoPanel = CurrentTileInfoPanel });

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
        CultureListPanel NewPanel = NewTileInfoPanel.GetComponentInChildren<CultureListPanel>();
        NewPanel.OnCultureButtonCreated += CultureListPanel_OnCultureButtonCreated;

        NewTileInfoPanel.GetComponent<TileInfoPanelController>().SetValues(SelectedTile);

        return NewTileInfoPanel;
    }

    private void CultureListButton_OnCultureButtonClicked(object sender, CultureListButton.OnCultureButtonClickedEventArgs e)
    {
        SelectNewCulture(e.Culture.gameObject);
    }

    void CultureListPanel_OnCultureButtonCreated(object sender, CultureListPanel.OnCultureButtonCreatedEventArgs e)
    {
        e.NewButton.GetComponent<CultureListButton>().OnCultureButtonClicked += CultureListButton_OnCultureButtonClicked;
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
            if(g.GetComponent<Culture>() != null && !Input.GetKey(KeyCode.D)) // deleting culture if "D" is held
            {
                SelectNewCulture(g);
            }
            
        }
    }

    void TileInfoPanelController_OnPanelDestroyed(object sender, EventArgs e)
    {
        StopListeningToOldTile();
        CurrentTileInfoPanel = null;
    }

    public class OnTileSelectedArgs : EventArgs
    {
        public GameObject SelectedTile;
        public GameObject SelectedTileInfoPanel;
    }
}
