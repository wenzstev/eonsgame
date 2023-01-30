using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileDrawer : MonoBehaviour
{
    TileChars tileChars;
    TileFood tileFood;
    SpriteRenderer sr;

    public BiomeType tileType;


    public Color tempMin;
    public Color tempMax;
    public Color precipitationMin;
    public Color precipitationMax;
    public Color waterColor;
    public Color frozenColor;

    public float tempMinValue = -15;
    public float tempMaxValue = 35;

    public float precipitationMinValue = 0;
    public float precipitationMaxValue = 450;


    public enum BiomeType
    {
        Desert,                     // temp above 5 degrees, precipitation 25 or below
        Savannah,                   // temp between 10 and 25, precipitation between 50 and 125 
        TropicalRainforest,         // temp above 18, precipitation greater than 168 cm
        Grassland,                  // temp between 5 and 20, precipitation between 25 and 50
        Woodland,                   // temp between 5 and 20, precipitation between 50 and 
        SeasonalForest,             //
        TemperateRainforest,        // temp between 5 and 18, precipitation greater than 168 cm
        Taiga,               // temp 0 - 5 degrees, precipitation 25 - 125
        Tundra,                     // temp 0 - 5 degrees, precipitation 15 - 25 c,
        Ice,                        // temp below 0, any precipitation
        Water,                      
        Barren                      // temp above 32
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

    BiomeType[,] oldBiomeTable = new BiomeType[6, 6]
    {
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland, BiomeType.Desert, BiomeType.Desert, BiomeType.Desert },
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland, BiomeType.Desert, BiomeType.Desert, BiomeType.Desert },
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland, BiomeType.Woodland, BiomeType.Savannah, BiomeType.Savannah },
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.Taiga, BiomeType.Woodland, BiomeType.Savannah, BiomeType.Savannah },
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.Taiga, BiomeType.SeasonalForest, BiomeType.TropicalRainforest, BiomeType.TropicalRainforest },
        {BiomeType.Ice, BiomeType.Tundra, BiomeType.Taiga, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest, BiomeType.TropicalRainforest },
    };

    BiomeType[,] BiomeTable = new BiomeType[9, 11]
    { //        -15                 -10                 -5              0                   5                               10                              15                              20                              25                              30                              35
    /* <50 */   {BiomeType.Tundra, BiomeType.Tundra, BiomeType.Tundra, BiomeType.Taiga,     BiomeType.Grassland,            BiomeType.Grassland,            BiomeType.Desert,               BiomeType.Desert,               BiomeType.Desert,               BiomeType.Desert,               BiomeType.Desert},
    /* <100 */  {BiomeType.Tundra, BiomeType.Tundra, BiomeType.Tundra, BiomeType.Taiga,     BiomeType.Grassland,            BiomeType.Grassland,            BiomeType.SeasonalForest,       BiomeType.Savannah,             BiomeType.Savannah,             BiomeType.Desert,               BiomeType.Savannah},
    /* <150 */  {BiomeType.Tundra, BiomeType.Tundra, BiomeType.Tundra, BiomeType.Taiga,     BiomeType.Taiga,                BiomeType.SeasonalForest,       BiomeType.SeasonalForest,       BiomeType.Savannah,             BiomeType.Savannah,             BiomeType.Woodland,             BiomeType.Woodland},
    /* <200 */  {BiomeType.Tundra, BiomeType.Tundra, BiomeType.Tundra, BiomeType.Taiga,     BiomeType.SeasonalForest,       BiomeType.SeasonalForest,       BiomeType.SeasonalForest,       BiomeType.Woodland,             BiomeType.Woodland,             BiomeType.Woodland,             BiomeType.Woodland},
    /* <250 */  {BiomeType.Tundra, BiomeType.Tundra, BiomeType.Tundra, BiomeType.Taiga,     BiomeType.TemperateRainforest,  BiomeType.TemperateRainforest,  BiomeType.TemperateRainforest,  BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest},
    /* <300 */  {BiomeType.Tundra, BiomeType.Tundra, BiomeType.Tundra, BiomeType.Taiga,     BiomeType.TemperateRainforest,  BiomeType.TemperateRainforest,  BiomeType.TemperateRainforest,  BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest},
    /* <350 */  {BiomeType.Tundra, BiomeType.Tundra, BiomeType.Tundra, BiomeType.Taiga,     BiomeType.TemperateRainforest,  BiomeType.TemperateRainforest,  BiomeType.TemperateRainforest,  BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest },
    /* <400 */  {BiomeType.Tundra, BiomeType.Tundra, BiomeType.Tundra, BiomeType.Taiga,     BiomeType.TemperateRainforest,  BiomeType.TemperateRainforest,  BiomeType.TemperateRainforest,  BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest},
    /* >400 */  {BiomeType.Tundra, BiomeType.Tundra, BiomeType.Tundra, BiomeType.Taiga,     BiomeType.TemperateRainforest,  BiomeType.TemperateRainforest,  BiomeType.TemperateRainforest,  BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest,   BiomeType.TropicalRainforest}
    };





    public void Initialize()
    {
        tileChars = GetComponent<TileChars>();
        tileFood = GetComponent<TileFood>();    

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

        int temperatureInt = Mathf.RoundToInt(temperature / 5f) + 3; // add three because we're offset from zero by 3 rows
        int precipitationInt = Mathf.RoundToInt(precipitation / 50f);


        if(precipitationInt >= 0 && precipitationInt < BiomeTable.GetLength(0) && temperatureInt >= 0 && temperatureInt < BiomeTable.GetLength(1))
            tileType = BiomeTable[precipitationInt, temperatureInt];


        // special cases
        if (temperature > tempMaxValue) tileType = BiomeType.Barren;
        if (tileChars.isUnderwater) tileType = BiomeType.Water;
        if (temperature < tempMinValue) tileType = BiomeType.Ice;
    

        
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
            case BiomeType.Taiga:
                sr.color = BorealForestColor;
                break;
            case BiomeType.SeasonalForest:
                sr.color = SeasonalForestColor;
                break;
            case BiomeType.TemperateRainforest:
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
