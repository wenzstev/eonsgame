using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckPlacer : MonoBehaviour
{
    public GameObject culturePuck;


    private void Start()
    {
        EventManager.StartListening("MouseDownTile", placePuck);
    }

    void placePuck(Dictionary<string, object> message)
    {
        GameObject tileClicked = (GameObject)message["tile"];
        GameObject puck = Instantiate(culturePuck, tileClicked.transform.position, Quaternion.identity);
        puck.GetComponent<Culture>().Init(tileClicked.GetComponent<Tile>());
        EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object> { { "culture", puck.GetComponent<Culture>() } });
    }
}
