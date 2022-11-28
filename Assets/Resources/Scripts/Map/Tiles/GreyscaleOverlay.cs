using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GreyscaleOverlay : MonoBehaviour
{
    SpriteRenderer _greyscaleSprite;

    const float RED_CONSTANT = .3f;
    const float GREEN_CONSTANT = .59f;
    const float BLUE_CONSTANT = .11f;


    /// <summary>
    /// Returns the greyscale value of the overlay sprite. Based on the color of the initialized sprite.
    /// </summary>
    public Color GreyscaleValue{
        get 
        {
            try
            {
                return new Color(_greyscaleSprite.color.r, _greyscaleSprite.color.g, _greyscaleSprite.color.b, 1);
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
    public float PercentCovered 
    {
        get 
        {
            return 1f;
        }
    }

    /// <summary>
    /// Initialize the Greyscale Overlay. Requires a target sprite to create the greyscale version of (uses the sprites's color).
    /// </summary>
    /// <param name="target">The sprite to take the color to turn greyscale.</param>
    public void Initialize(SpriteRenderer target)
    {
        _greyscaleSprite = GetComponent<SpriteRenderer>();
        Color targetColor = target.color;
        Color greyscaleColor = GetGreyscaleColor(targetColor);
        _greyscaleSprite.color = greyscaleColor;

    }

    /// <summary>
    /// Set the value of the greyscale overlay. Higher value means more grey.
    /// </summary>
    /// <param name="percent">Value (between 0 and 1) to determine how grey the overlay is.</param>
    public void SetGreyscalePercentage(float percent)
    {

    }
    
    Color GetGreyscaleColor(Color c)
    {
        return new Color(c.r * RED_CONSTANT, c.g * GREEN_CONSTANT, c.b * BLUE_CONSTANT);
    }
    
}


public class GameObjectNotInitializedException : Exception 
{
    public GameObjectNotInitializedException() : base() { }
    public GameObjectNotInitializedException(string message) : base(message) { }

}