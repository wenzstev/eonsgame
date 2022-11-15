using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAmountIndicatorGenerator : MonoBehaviour
{
    public GameObject FoodAmountIndicatorTemplate;
    CultureFoodStore FoodStore;
    public int TicksBetweenIndicators = 1;

    float _amountFoodChange = 0;
    int _numTicksSinceIndicator;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("Tick", OnTick);
        FoodStore = transform.parent.GetComponent<CultureFoodStore>();
        _numTicksSinceIndicator = 0;
    }

    void OnTick(Dictionary<string, object> empty)
    {
        _amountFoodChange += FoodStore.LastTickChange;
        _numTicksSinceIndicator++;
        if (_numTicksSinceIndicator == TicksBetweenIndicators) CreateIndicator();
    }

    void CreateIndicator()
    {
        GameObject Indicator = Instantiate(FoodAmountIndicatorTemplate, transform);
        Indicator.GetComponent<FoodAmountIndicator>().Initialize(_amountFoodChange);
        _numTicksSinceIndicator = 0;
        _amountFoodChange = 0;
    }

    private void OnDestroy()
    {
        EventManager.StopListening("Tick", OnTick);
    }


}
