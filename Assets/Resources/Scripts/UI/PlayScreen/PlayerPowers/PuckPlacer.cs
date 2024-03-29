using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckPlacer : MonoBehaviour
{
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
            AttemptPlacement(e.GetFirstThatContains<Tile>());
        }
    }

    void AttemptPlacement(GameObject TileClicked)
    {
        if (TileClicked.GetComponent<TileChars>().Biome == TileDrawer.BiomeType.Water) return; 
        Culture puck = CulturePool.GetCulture();
        puck.transform.position = TileClicked.transform.position;
        puck.Init(TileClicked.GetComponent<Tile>(), initialPopulation);
        EventManager.TriggerEvent("CultureUpdated" + puck.GetComponent<Culture>().name, new Dictionary<string, object> { { "culture", puck.GetComponent<Culture>() } });
        

    }
}
