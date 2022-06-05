using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CulturePanelCreator : MonoBehaviour
{

    public GameObject cultureInfoPanel;

    public GameObject Viewport;

    

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("CultureAggregateAdded", AddNewInfoPanel);
    }

    public void AddNewInfoPanel(Dictionary<string, object> infoPanelDict)
    {
        GameObject newInfoPanel = Instantiate(cultureInfoPanel);
        newInfoPanel.transform.SetParent(Viewport.transform);


        CultureAggregation ca = (CultureAggregation)infoPanelDict["cultureAggregate"];
        newInfoPanel.GetComponent<CultureAggregateInfo>().Init(ca);
    }

}
