using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{

    public static ViewController Instance;
    public Views view;

    public enum Views
    {
        Terrain,
        Culture,
        Population
    }

    private void Awake()
    {
        Instance = this;
        EventManager.StartListening("ChangeViewMode", updateView);
    }



    public void updateView(Dictionary<string, object> newView)
    {
        view = (Views) newView["view"];
    }
}



