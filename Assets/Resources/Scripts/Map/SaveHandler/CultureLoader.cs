using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureLoader : MonoBehaviour
{
    public GameObject CultureTile;
    public void LoadCulturesFromSerialized(SerializedCultures scs, GameObject boardObj)
    {
        Board b = boardObj.GetComponent<Board>();
        foreach (SerializedCulture sc in scs.cultures)
        {
            // assuming that the tile has been created
            GameObject curTile = b.GetTile(sc.x, sc.y);
            GameObject curCultureObj = Instantiate(CultureTile, curTile.transform);
            JsonUtility.FromJsonOverwrite(sc.serializedComponents[0], curCultureObj.GetComponent<Culture>());
            JsonUtility.FromJsonOverwrite(sc.serializedComponents[1], curCultureObj.GetComponent<CultureMemory>());


            Culture curCulture = curCultureObj.GetComponent<Culture>();
            curCulture.LoadFromSerialized(curTile, CultureTile);
            EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object> { { "culture", curCulture.name } });
            EventManager.TriggerEvent("CultureUpdated" + curCulture.name, new Dictionary<string, object> { { "culture", curCulture } });
        }
    }
}
