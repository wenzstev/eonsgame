using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CultureController : MonoBehaviour
{
    List<CultureBrain> AllCultures;
    List<CultureBrain> _culturesToExecute;

    private void Start()
    {
        AllCultures = new List<CultureBrain>();

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
        AllCultures.Add(c.GetComponent<CultureBrain>());
    }

    void OnCultureDestroyed(Dictionary<string, object> destroyedCulture)
    {
        Culture c = (Culture) destroyedCulture["culture"];
        //Debug.Log($"Removing {c} from CultureList");

        AllCultures.Remove(c.GetComponent<CultureBrain>());
    }

    public Culture[] GetAllCultures()
    {
        return AllCultures.Select(c => c.Culture).ToArray();
    }

    private void OnDestroy()
    {
        EventManager.StopListening("Tick", OnTick);
    }


    void ExecuteAllCultureTurns()
    {
        CultureBrain[] culturesToExecute = AllCultures.ToArray();
        foreach(CultureBrain c in culturesToExecute)
        {
            if(c.isActiveAndEnabled) c.ExecuteCultureTurn();
        }
    }

}
