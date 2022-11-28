using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GreyscaleOverlay : MonoBehaviour
{
    SpriteRenderer _greyscaleSprite;
    Color _greyscaleColor;
    float currentTransparency;


    /// <summary>
    /// Returns the greyscale value of the overlay sprite. Based on the color of the initialized sprite.
    /// </summary>
    public Color GreyscaleColor{
        get 
        {
            try
            {
                return _greyscaleColor;
            }
            catch (NullReferenceException)
            {
                throw new GameObjectNotInitializedException("GreyscaleOverlay is being used without being initialized!");

            }
        }
    }

    /// <summary>
    /// Returns the percentage that the greyscale value has covered the original value. 
    /// </summary>
    public float PercentTransparent 
    {
        get 
        {
            return currentTransparency;
        }
    }

    /// <summary>
    /// Initialize the Greyscale Overlay. Requires a target sprite to create the greyscale version of (uses the sprites's color).
    /// </summary>
    /// <param name="target">The sprite to take the color to turn greyscale.</param>
    public void Initialize(SpriteRenderer target)
    {
        _greyscaleSprite = GetComponent<SpriteRenderer>();
        _greyscaleColor = GetGreyscaleColor(target.color);
        _greyscaleSprite.color = GetGreyscaleColorTransparent(_greyscaleColor, 0);

    }

    /// <summary>
    /// Set the value of the greyscale overlay. Higher value means more grey.
    /// </summary>
    /// <param name="percent">Value (between 0 and 1) to determine how grey the overlay is.</param>
    public void SetGreyscalePercentage(float percent)
    {
        if (percent < 0 || percent > 1) throw new ArgumentException("Percent must be between 0 and 1!");
        _greyscaleSprite.color = GetGreyscaleColorTransparent(_greyscaleColor, percent);
        currentTransparency = percent;

    }

    Color GetGreyscaleColor(Color c)
    {
        return new Color(c.grayscale, c.grayscale, c.grayscale, 1f);
    }

    Color GetGreyscaleColorTransparent(Color c, float alpha)
    {
        return new Color(c.grayscale, c.grayscale, c.grayscale, alpha);
    }
    
}


public class GameObjectNotInitializedException : Exception 
{
    public GameObjectNotInitializedException() : base() { }
    public GameObjectNotInitializedException(string message) : base(message) { }

}