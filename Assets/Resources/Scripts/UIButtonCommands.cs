using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonCommands : MonoBehaviour
{
    public void SetTerrainMode()
    {
        EventManager.TriggerEvent("ChangeViewMode", new Dictionary<string, object> { { "view", ViewController.Views.Terrain } } );
    }

    public void SetCultureMode()
    {
        EventManager.TriggerEvent("ChangeViewMode", new Dictionary<string, object> { { "view", ViewController.Views.Culture } } );
    }

    public void SetPopulationMode()
    {
        EventManager.TriggerEvent("ChangeViewMode", new Dictionary<string, object> { { "view", ViewController.Views.Population } });
    }
}