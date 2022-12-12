using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckPlacer : MonoBehaviour
{
    public GameObject culturePuck;
    public int initialPopulation;


    private void Start()
    {
        EventManager.StartListening("InteractiveMouseUp", placePuck);
    }

    void placePuck(Dictionary<string, object> message)
    {
        //Debug.Log($"{ gameObject.GetHashCode()} is placing a tile");
        if(Input.GetKey("space"))
        {
            GameObject tileClicked = (GameObject)message["tile"];
            GameObject puck = Instantiate(culturePuck, tileClicked.transform.position, Quaternion.identity);
            puck.GetComponent<Culture>().Init(tileClicked.GetComponent<Tile>(), initialPopulation);
            EventManager.TriggerEvent("CultureUpdated" + puck.GetComponent<Culture>().name, new Dictionary<string, object> { { "culture", puck.GetComponent<Culture>() } });
        }

    }

    private void OnDestroy()
    {
        EventManager.StopListening("InteractiveMouseUp", placePuck);
    }
}
