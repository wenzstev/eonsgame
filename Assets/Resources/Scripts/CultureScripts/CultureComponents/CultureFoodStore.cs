using UnityEngine;
using System;

public class CultureFoodStore : MonoBehaviour 
{
    [SerializeField]
    float currentFoodStore;
    public float CurrentFoodStore
    {
        get
        {
            return currentFoodStore;
        }
    }
    public float LastTickChange { get; private set; }

    public void AlterFoodStore(float lastTickChange)
    {
        LastTickChange = lastTickChange;
        currentFoodStore += LastTickChange;
    }
}