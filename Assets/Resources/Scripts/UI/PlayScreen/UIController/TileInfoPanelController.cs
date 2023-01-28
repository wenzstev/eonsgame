using System;
using System.Linq;
using UnityEngine;
using TMPro;

public class TileInfoPanelController : MonoBehaviour
{
    public TextMeshProUGUI TileName;
    public TextMeshProUGUI TileFood;
    public TextMeshProUGUI TilePopulation;

    TileChars SelectedTileChars;
    TileFood SelectedTileFood;
    CultureHandler SelectedTileCultures;


    public event EventHandler<EventArgs> OnPanelDestroyed;

    public CultureListPanel CultureListPanel;
    public TileCharsPanelController TileCharsPanelController;

    public void SetValues(GameObject SelectedTile)
    {
        DeselectPreviousTile();

        SelectedTileChars = SelectedTile.GetComponent<TileChars>();
        SelectedTileFood = SelectedTile.GetComponent<TileFood>();
        SelectedTileCultures = SelectedTile.GetComponentInChildren<CultureHandler>();

        SetBiomeText(SelectedTileChars.Biome);
        if(SelectedTileChars.Biome != TileDrawer.BiomeType.Water) SetFoodAmount(SelectedTileFood.CurFood);
        UpdateToCurrentPopulation();
        CultureListPanel.Initialize(SelectedTile);

        TileCharsPanelController.SetValues(SelectedTileChars);


        SelectedTileFood.OnFoodChange += TileInfoPanelController_OnFoodChange;
        SelectedTileCultures.OnPopulationChanged += TileInfoPanelController_OnPopulationChanged;

        GetComponentInChildren<ZoomToObj>().SetSelectedObj(SelectedTile);
    }

    void SetBiomeText(TileDrawer.BiomeType biome)
    {
        TileName.text = DisplayUtils.SplitCamelCase(biome.ToString());
    }

    void DeselectPreviousTile()
    {
        if(SelectedTileFood) SelectedTileFood.OnFoodChange -= TileInfoPanelController_OnFoodChange;
        if(SelectedTileCultures) SelectedTileCultures.OnPopulationChanged -= TileInfoPanelController_OnPopulationChanged;


        SelectedTileChars = null;
        SelectedTileFood = null;
        SelectedTileCultures = null;
    }

    int CalculateTilePopulation()
    {
        return SelectedTileCultures.GetAllCultures().Sum(c => c.Population);
    }

    void SetFoodAmount(float foodAmount)
    {
        string foodString = foodAmount > 1000 ? DisplayUtils.RoundToKilo(foodAmount) + "k" : Mathf.RoundToInt(foodAmount).ToString();
        TileFood.text = foodString;
    }

    void UpdateToCurrentPopulation()
    {
        int pop = CalculateTilePopulation();
        string popString = pop > 1000 ? DisplayUtils.RoundToKilo(pop) + "k" : pop.ToString();
        TilePopulation.text = popString;
    }

    void TileInfoPanelController_OnFoodChange(object sender, TileFood.OnFoodChangeEventArgs e)
    {
        SetFoodAmount(e.CurFood);
    }

    void TileInfoPanelController_OnPopulationChanged(object sender, CultureHandler.OnPopulationChangedEventArgs e)
    {
        UpdateToCurrentPopulation();
    }

    public void DestroyPanel()
    {
        Destroy(gameObject);
        OnPanelDestroyed?.Invoke(this, EventArgs.Empty);
    }

}
