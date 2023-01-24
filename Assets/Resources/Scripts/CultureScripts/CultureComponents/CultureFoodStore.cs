using UnityEngine;
using System;

public class CultureFoodStore : MonoBehaviour 
{
    [SerializeField]
    float currentFoodStore;

    Culture _culture;

    public float StorePerPopulation = 10;
    public event EventHandler<OnFoodStoreChangedEventArgs> OnFoodStoreChanged;

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
            return _culture.Population * StorePerPopulation;
        }
    }

    private void Awake()
    {
        _culture = GetComponent<Culture>();
    }

    public void AlterFoodStore(float lastTickChange)
    {
        LastTickChange = lastTickChange;
        currentFoodStore += LastTickChange;
        currentFoodStore = Mathf.Max(0, currentFoodStore);
        OnFoodStoreChanged?.Invoke(this, new OnFoodStoreChangedEventArgs() { FoodChangeAmount = lastTickChange });
    }

    public class OnFoodStoreChangedEventArgs : EventArgs
    {
        public float FoodChangeAmount;
    }
}