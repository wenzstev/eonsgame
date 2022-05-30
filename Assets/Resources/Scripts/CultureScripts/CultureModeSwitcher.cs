using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureModeSwitcher : MonoBehaviour
{

    GameObject terrainView;
    GameObject cultureView;
    GameObject populationView;
    GameObject highlightView;

    GameObject[] views;

    ViewController.Views currentView;

    // Start is called before the first frame update
    void Start()
    {
        terrainView = transform.GetChild(0).gameObject;
        cultureView = transform.GetChild(1).gameObject;
        populationView = transform.GetChild(2).gameObject;
        highlightView = transform.GetChild(3).gameObject;

        views = new GameObject[] { terrainView, cultureView, populationView, highlightView };

        currentView = ViewController.Views.Culture;

        EventManager.StartListening("ChangeViewMode", SetView);
        EventManager.StartListening("CurrentView", GetViewOnStart);
        EventManager.TriggerEvent("CurrentViewRequested", null);
    }

    void SetView(Dictionary<string, object> newView)
    {
        //Debug.Log(GetHashCode() + ": setting view to " + newView["view"]);
        views[(int)currentView].SetActive(false);
        views[(int)newView["view"]].SetActive(true);
        currentView = (ViewController.Views) newView["view"];
    }

    void GetViewOnStart(Dictionary<string, object> currentView)
    {
        SetView(currentView);
        EventManager.StopListening("CurrentView", GetViewOnStart);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("ChangeViewMode", SetView);
    }



}
