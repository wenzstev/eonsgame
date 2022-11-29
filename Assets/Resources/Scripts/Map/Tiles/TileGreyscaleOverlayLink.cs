using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileGreyscaleOverlayLink : MonoBehaviour
{
    public SpriteRenderer targetSprite;
    public TileFood _tileFood;
    GreyscaleOverlay _greyscaleOverlay;
    float percentLowIndicator = .2f;


    void Start()
    {
        Initialize();    
    }

    public void Initialize()
    {
        _tileFood = GetComponentInParent<TileFood>();
        _tileFood.OnLowFood += TileGreyscaleOverlayLink_OnLowFood;
        _greyscaleOverlay = GetComponent<GreyscaleOverlay>();
        _greyscaleOverlay.Initialize(targetSprite);
    }

    private void TileGreyscaleOverlayLink_OnLowFood(object sender, TileFood.OnLowFoodEventArgs e)
    {
        if(e.CurFood / e.MaxFood < percentLowIndicator)
        {
            AdjustOverlayTransparency(e.CurFood, e.MaxFood);
        }
        else if(_greyscaleOverlay.PercentTransparent != 0)
        {
            MakeOverlayTransparent();
        }
    }

    void AdjustOverlayTransparency(float curFood, float maxFood)
    {
        // determine the percentage, from 0% to percentLowIndicator (anything above percentLowIndicator is not overlaid)
        float percentOfMaxFood = maxFood * percentLowIndicator;
        if (percentOfMaxFood < curFood) throw new ArgumentException("Cur food is higher than percent of max food!");
        _greyscaleOverlay.SetGreyscalePercentage(Mathf.InverseLerp(percentOfMaxFood, 0, curFood));
    }

    void MakeOverlayTransparent()
    {
        _greyscaleOverlay.SetGreyscalePercentage(0);
    }

    private void OnDestroy()
    {
        _tileFood.OnLowFood -= TileGreyscaleOverlayLink_OnLowFood;
    }

}
