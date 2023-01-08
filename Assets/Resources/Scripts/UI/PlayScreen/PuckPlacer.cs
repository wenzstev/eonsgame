using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckPlacer : MonoBehaviour
{
    public GameObject culturePuck;
    public int initialPopulation;
    public MouseActionsController MouseActionsController;


    private void Awake()
    {
        MouseActionsController.MouseUpAction += MouseActionsController_MouseUpAction;
    }

    private void MouseActionsController_MouseUpAction(object sender, MouseActionsController.MouseActionEventArgs e)
    {
        if(Input.GetKey("space") && e.GetFirstThatContains<Tile>() != null)
        {
            PlacePuck(e.GetFirstThatContains<Tile>());
        }
    }

    void PlacePuck(GameObject TileClicked)
    {
        GameObject puck = Instantiate(culturePuck, TileClicked.transform.position, Quaternion.identity);
        puck.GetComponent<Culture>().Init(TileClicked.GetComponent<Tile>(), initialPopulation);
        EventManager.TriggerEvent("CultureUpdated" + puck.GetComponent<Culture>().name, new Dictionary<string, object> { { "culture", puck.GetComponent<Culture>() } });
        

    }
}
