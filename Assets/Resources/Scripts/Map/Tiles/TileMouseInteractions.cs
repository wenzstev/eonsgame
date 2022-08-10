using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMouseInteractions : MonoBehaviour
{

    GameObject tileActive;
    TileInfo ti;

    private void Start()
    {
        tileActive = transform.GetChild(0).gameObject;
        ti = GetComponent<TileInfo>();
    }

    private void OnMouseEnter()
    {
        EventManager.TriggerEvent("MouseOnTile", new Dictionary<string, object> { { "TileInfo", ti } });
        tileActive.SetActive(true);
    }

    private void OnMouseExit()
    {
        tileActive.SetActive(false);
    }

    private void OnMouseUp()
    {
        EventManager.TriggerEvent("MouseUpTile", new Dictionary<string, object> { { "tile", gameObject } });
    }

}
