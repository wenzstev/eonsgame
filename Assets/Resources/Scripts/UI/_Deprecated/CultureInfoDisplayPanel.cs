using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CultureInfoDisplayPanel : MonoBehaviour
{

    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        EventManager.StartListening("MouseOnTile", OnMouseOnTile);
    }

    void OnMouseOnTile(Dictionary<string, object> cultureInfo)
    {
        text.text = "";
        TileInfo info = (TileInfo)cultureInfo["TileInfo"];
        foreach(Culture c in info.cultures.Values)
        {
            text.text += c.name + "\nPopulation: " + c.Population + "\n" + "Affinity: " + c.affinity + "\n\n";
        }
    }

    private void OnDestroy()
    {
        EventManager.StartListening("MouseOnTile", OnMouseOnTile);
    }

}
