using UnityEngine;
using System;

public class CultureFoodStore : MonoBehaviour 
{
    [SerializeField]
    float currentFoodStore;

    Culture _culture;

    public float StorePerPopulation = 10;
    public event EventHandler<OnFoodStoreChangedEventArgs> OnFoodStoreChanged;

    OnFoodStoreChangedEventArgs onFoodStoreChangedEventArgs;

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
        onFoodStoreChangedEventArgs = new OnFoodStoreChangedEventArgs();
    }

    public void AlterFoodStore(float lastTickChange)
    {
        LastTickChange = lastTickChange;
        currentFoodStore += LastTickChange;
        currentFoodStore = Mathf.Max(0, currentFoodStore);
        onFoodStoreChangedEventArgs.FoodChangeAmount = lastTickChange;
        OnFoodStoreChanged?.Invoke(this, onFoodStoreChangedEventArgs);
    }

    public class OnFoodStoreChangedEventArgs : EventArgs
    {
        public float FoodChangeAmount;
    }
}