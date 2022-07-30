using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureLoader : MonoBehaviour
{
    public Board b;
    public GameObject CultureTile;
    public void CreateCultures(List<SerializedTile> tiles)
    { 
        foreach(SerializedTile tile in tiles)
        {
            foreach (SerializedCulture sc in tile.cultures)
            {
                // assuming that the tile has been created
                GameObject curTile = b.GetTile(tile.x, tile.y);
                GameObject curCultureObj = Instantiate(CultureTile, curTile.transform.position, Quaternion.identity);
                Culture curCulture = curCultureObj.GetComponent<Culture>();
                curCulture.LoadFromSave(sc, curTile.GetComponent<Tile>());
                EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object> { { "culture", curCulture.name } });
                EventManager.TriggerEvent("CultureUpdated" + curCulture.name, new Dictionary<string, object> { { "culture", curCulture } });
            }
        }
    }

}
