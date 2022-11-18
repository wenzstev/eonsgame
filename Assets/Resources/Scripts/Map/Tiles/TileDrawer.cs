using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDrawer : MonoBehaviour
{
    TileChars tileChars;
    SpriteRenderer sr;

    public BiomeType tileType;


    public Color tempMin;
    public Color tempMax;
    public Color precipitationMin;
    public Color precipitationMax;
    public Color waterColor;
    public Color frozenColor;

    public float tempMinValue;
    public float tempMaxValue;

    public float precipitationMinValue;
    public float precipitationMaxValue;

    public enum BiomeType
    {
        Desert,
        Savannah,
        TropicalRainforest,
        Grassland,
        Woodland,
        SeasonalForest,
        TemperateRainForest,
        BorealForest,
        Tundra,
        Ice,
        Water,
        Barren
    }

    public Color DesertColor;
    public Color SavannahColor;
    public Color TropicalRainforestColor;
    public Color GrasslandColor;
    public Color WoodlandColor;
    public Color SeasonalForestColor;
    public Color TemperateRainForestColor;
    public Color BorealForestColor;
    public Color TundraColor;
    public Color IceColor;
    public Color WaterColor;
    public Color BarrenColor;

    BiomeType[,] BiomeTable = new BiomeType[6, 6]
    {
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland, BiomeType.Desert, BiomeType.Desert, BiomeType.Desert },
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland, BiomeType.Desert, BiomeType.Desert, BiomeType.Desert },
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland, BiomeType.Woodland, BiomeType.Savannah, BiomeType.Savannah },
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland, BiomeType.Savannah, BiomeType.Savannah },
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest, BiomeType.TropicalRainforest, BiomeType.TropicalRainforest },
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainForest, BiomeType.TropicalRainforest, BiomeType.TropicalRainforest },
    };

    // Start is called before the first frame update
    void Start()
    {
        tileChars = GetComponent<TileChars>();
        sr = GetComponent<SpriteRenderer>();
        DetermineBiomeAndColor();
    }

    public void UpdateDraw()
    {
        DetermineBiomeAndColor();
    }

    void DetermineBiomeAndColor()
    {
        float precipitation = tileChars.precipitation;
        float temperature = tileChars.temperature;

        int temperatureInt = Mathf.FloorToInt(Mathf.InverseLerp(tempMinValue, tempMaxValue, temperature) * (BiomeTable.GetLength(0)-1));
        int precipitationInt = Mathf.FloorToInt(Mathf.InverseLerp(precipitationMinValue, precipitationMaxValue, precipitation) * (BiomeTable.GetLength(1)-1));



        tileType = tileChars.precipitation <= 10 ? BiomeType.Barren : tileChars.isUnderwater ? BiomeType.Water : BiomeTable[precipitationInt, temperatureInt];
        
        switch(tileType)
        {
            case BiomeType.Desert:
                sr.color = DesertColor;
                break;
            case BiomeType.Savannah:
                sr.color = SavannahColor;
                break;
            case BiomeType.Grassland:
                sr.color = GrasslandColor;
                break;
            case BiomeType.Woodland:
                sr.color = WoodlandColor;
                break;
            case BiomeType.BorealForest:
                sr.color = BorealForestColor;
                break;
            case BiomeType.SeasonalForest:
                sr.color = SeasonalForestColor;
                break;
            case BiomeType.TemperateRainForest:
                sr.color = TemperateRainForestColor;
                break;
            case BiomeType.TropicalRainforest:
                sr.color = TropicalRainforestColor;
                break;
            case BiomeType.Tundra:
                sr.color = TundraColor;
                break;
            case BiomeType.Ice:
                sr.color = IceColor;
                break;
            case BiomeType.Water:
                sr.color = WaterColor;
                break;
            case BiomeType.Barren:
                sr.color = BarrenColor;
                break;
        }

    }
}
