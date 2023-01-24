using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureLoader : MonoBehaviour
{
    public GameObject CultureTile;
    public void LoadCulturesFromSerialized(SerializedCultures scs, GameObject boardObj)
    {
        Board b = boardObj.GetComponent<Board>();
        foreach (SerializedCulture sc in scs.SettledCultures)
        {
            // assuming that the tile has been created
            GameObject curTile = b.GetTile(sc.x, sc.y);
            Culture NewCulture = DeserializeCulture(sc, curTile);

            NewCulture.SetTile(curTile.GetComponent<Tile>(), true);
        }

        foreach(SerializedCulture sc in scs.StagedCultures)
        {
            GameObject curTile = b.GetTile(sc.x, sc.y);
            Culture NewCulture = DeserializeCulture(sc, curTile);

            NewCulture.SetTile(curTile.GetComponent<Tile>(), false);
        }

    }


    Culture DeserializeCulture(SerializedCulture sc, GameObject tile)
    {
        Culture NewCulture = CulturePool.GetCulture(); 
        GameObject curCultureObj = NewCulture.gameObject;
        NewCulture.transform.position = tile.transform.position;

        JsonUtility.FromJsonOverwrite(sc.serializedComponents[0], curCultureObj.GetComponent<Culture>());
        JsonUtility.FromJsonOverwrite(sc.serializedComponents[1], curCultureObj.GetComponent<CultureMemory>());
        JsonUtility.FromJsonOverwrite(sc.serializedComponents[2], curCultureObj.GetComponent<AffinityManager>());

        NewCulture.SetColor(NewCulture.Color);
        NewCulture.RenameCulture(NewCulture.Name);

        EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object>() { { "culture", curCultureObj.GetComponent<Culture>() } });
        return curCultureObj.GetComponent<Culture>();
    }
}
