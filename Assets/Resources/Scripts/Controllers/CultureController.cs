using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureController : MonoBehaviour
{
    List<Culture> AllCultures;

    private void Start()
    {
        AllCultures = new List<Culture>();

        EventManager.StartListening("Tick", OnTick);
        EventManager.StartListening("CultureCreated", OnCultureCreated);
        EventManager.StartListening("CultureDestroyed", OnCultureDestroyed);
    }

    void OnTick(Dictionary<string, object> empty)
    {
        ExecuteAllCultureTurns();
    }

    void OnCultureCreated(Dictionary<string, object> createdCulture)
    {
        Culture c = (Culture) createdCulture["culture"];
        AllCultures.Add(c);
    }

    void OnCultureDestroyed(Dictionary<string, object> destroyedCulture)
    {
        Culture c = (Culture) destroyedCulture["culture"];
        Debug.Log($"Removing {c} from CultureList");

        AllCultures.Remove(c);
    }

    public Culture[] GetAllCultures()
    {
        return AllCultures.ToArray();
    }


    void ExecuteAllCultureTurns()
    {
        Culture[] culturesToUpdate = AllCultures.ToArray(); 
        foreach(Culture c in culturesToUpdate)
        {
            c.DecisionMaker.ExecuteTurn();
            Turn.UpdateAllCultures();
        }
    }

}
