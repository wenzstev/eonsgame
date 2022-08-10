using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileInfoDisplayPanel : MonoBehaviour
{

    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        EventManager.StartListening("MouseOnTile", OnMouseOverTile);
    }

    void OnMouseOverTile(Dictionary<string, object> message)
    {
        TileInfo tileInfo = (TileInfo) message["TileInfo"];
        text.text = tileInfo.tileType.ToString();
    }

}
