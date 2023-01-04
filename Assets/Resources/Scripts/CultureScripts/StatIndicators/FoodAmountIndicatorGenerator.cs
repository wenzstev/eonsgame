using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodAmountIndicatorGenerator : MonoBehaviour
{
    public GameObject FoodAmountIndicatorTemplate;
    public CultureFoodStore FoodStore;
    public int CurTicksBetweenIndicators = 1;

    public int[] TicksBetweenIndicators;

    float _amountFoodChange = 0;
    int _numTicksSinceIndicator;

    void Awake()
    {
        FoodStore.OnFoodStoreChanged += FoodAmountIndicatorGenerator_OnFoodStoreChanged;
        EventManager.StartListening("SpeedChange", ChangeTicksBetween);
        _numTicksSinceIndicator = 0;
        CurTicksBetweenIndicators = TicksBetweenIndicators[TimeController.GetCurTimeLevel()];
    }


    void ChangeTicksBetween(Dictionary<string, object> newSpeed)
    {
        _numTicksSinceIndicator = 0;
        CurTicksBetweenIndicators = TicksBetweenIndicators[(int)newSpeed["speed"]];
    }

    void CreateIndicator()
    {
        GameObject Indicator = Instantiate(FoodAmountIndicatorTemplate, transform);
        Indicator.GetComponent<FoodAmountIndicator>().Initialize(_amountFoodChange);
        _numTicksSinceIndicator = 0;
        _amountFoodChange = 0;
    }

    public void TickExecuted()
    {
        _numTicksSinceIndicator++;
        if (_numTicksSinceIndicator == CurTicksBetweenIndicators) CreateIndicator();
    }

    void FoodAmountIndicatorGenerator_OnFoodStoreChanged(object sender, CultureFoodStore.OnFoodStoreChangedEventArgs e)
    {
        _amountFoodChange += e.FoodChangeAmount;
    }

}
