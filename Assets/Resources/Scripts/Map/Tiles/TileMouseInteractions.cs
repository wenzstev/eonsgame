using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMouseInteractions : MonoBehaviour
{
    TileInfo ti;

    private void Start()
    {
        ti = GetComponent<TileInfo>();
    }

    private void OnMouseEnter()
    {
        EventManager.TriggerEvent("MouseOnTile", new Dictionary<string, object> { { "TileInfo", ti } });
    }

    private void OnMouseUp()
    {
        EventManager.TriggerEvent("MouseUpTile", new Dictionary<string, object> { { "tile", gameObject } });
    }

}
