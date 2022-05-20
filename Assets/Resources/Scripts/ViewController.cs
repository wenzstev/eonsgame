using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{

    public static ViewController Instance;
    public Views view;
    Views previousView;

    public enum Views
    {
        Terrain,
        Culture,
        Population, 
        Highlight
    }

    private void Awake()
    {
        Instance = this;
        EventManager.StartListening("ChangeViewMode", updateView);
        EventManager.StartListening("HoverOff", RemoveHighlightView);
    }


    public void updateView(Dictionary<string, object> newView)
    {
        if ((Views)newView["view"] == Views.Highlight)
        {
            previousView = view;
        }

        view = (Views) newView["view"];

    }

    public void RemoveHighlightView(Dictionary<string, object> empty)
    {
        view = previousView;
        EventManager.TriggerEvent("ChangeViewMode", new Dictionary<string, object> { { "view", view } });
    }
}



