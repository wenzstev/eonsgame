using UnityEngine;
using System;

public class CultureFoodStore : MonoBehaviour 
{
    [SerializeField]
    float currentFoodStore;

    public float StorePerPopulation = 10;

    public float CurrentFoodStore
    {
        get
        {
            return currentFoodStore;
        }
    }
    public float LastTickChange { get; private set; }
    public float MaxFoodStore 
    {
        get
        {
            return GetComponent<Culture>().Population * StorePerPopulation;
        }
    }

    public void AlterFoodStore(float lastTickChange)
    {
        LastTickChange = lastTickChange;
        currentFoodStore += LastTickChange;
        currentFoodStore = Mathf.Max(0, currentFoodStore);
    }
}