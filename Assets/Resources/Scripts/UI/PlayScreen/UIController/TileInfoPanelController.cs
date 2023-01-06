using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileInfoPanelController : MonoBehaviour
{
    public TextMeshProUGUI TileName;
    public TextMeshProUGUI TileFood;
    public TextMeshProUGUI TilePopulation;

    TileChars SelectedTileChars;
    TileFood SelectedTileFood;
    CultureHandler SelectedTileCultures;



    public CultureListPanel CultureListPanel;

    public void SetValues(GameObject SelectedTile)
    {
        DeselectPreviousTile();

        SelectedTileChars = SelectedTile.GetComponent<TileChars>();
        SelectedTileFood = SelectedTile.GetComponent<TileFood>();
        SelectedTileCultures = SelectedTile.GetComponentInChildren<CultureHandler>();

        TileName.text = SelectedTileChars.Biome.ToString();
        SetFoodAmount(SelectedTileFood.CurFood);
        UpdateToCurrentPopulation();
        CultureListPanel.Initialize(SelectedTile);


        SelectedTileFood.OnFoodChange += TileInfoPanelController_OnFoodChange;
        SelectedTileCultures.OnPopulationChanged += TileInfoPanelController_OnPopulationChanged;
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
        string foodString = foodAmount > 1000 ? RoundToKilo(foodAmount) + "k" : Mathf.RoundToInt(foodAmount).ToString();
        TileFood.text = foodString;
    }

    void UpdateToCurrentPopulation()
    {
        int pop = CalculateTilePopulation();
        string popString = pop > 1000 ? RoundToKilo(pop) + "k" : pop.ToString();
        TilePopulation.text = popString;
    }

    void TileInfoPanelController_OnFoodChange(object sender, TileFood.OnFoodChangeEventArgs e)
    {
        SetFoodAmount(e.CurFood);
    }

    void TileInfoPanelController_OnPopulationChanged(object sender, CultureHandler.OnPopulationChangedEventArgs e)
    {
        CalculateTilePopulation();
    }

    static float RoundToKilo(float val)
    {
        return Mathf.RoundToInt(val / 100) / 10f;
    }
}
