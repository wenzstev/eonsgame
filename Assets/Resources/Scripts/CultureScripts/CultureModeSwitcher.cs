using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureModeSwitcher : MonoBehaviour
{

    GameObject terrainView;
    GameObject cultureView;
    GameObject populationView;

    // Start is called before the first frame update
    void Start()
    {
        cultureView = transform.GetChild(0).gameObject;
        terrainView = transform.GetChild(1).gameObject;
        populationView = transform.GetChild(2).gameObject;



        SetView(new Dictionary<string, object> { { "view", ViewController.Instance.view } });

        EventManager.StartListening("ChangeViewMode", SetView);
    }

    void SetView(Dictionary<string, object> newView)
    {
        switch (newView["view"])
        {
            case ViewController.Views.Culture:
                cultureView.SetActive(true);
                terrainView.SetActive(false);
                populationView.SetActive(false);
                break;
            case ViewController.Views.Terrain:
                cultureView.SetActive(false);
                terrainView.SetActive(true);
                populationView.SetActive(false);
                break;
            case ViewController.Views.Population:
                cultureView.SetActive(false);
                terrainView.SetActive(false);
                populationView.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(newView["view"].GetType().Name, newView["view"], null);
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening("ChangeViewMode", SetView);
    }



}
