using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCultureHighlightWhenClicked : MonoBehaviour
{
    public GameObject CultureHighlightPanelPefab;

    GameObject currentHighlightPanel;

    private void OnMouseUp() // TODO: change this to an event so that it can be combined with the main system
    {
        if(currentHighlightPanel == null){
            CreateNewPanel();
            return;
        } 
        DestroyPanel();
    }

    void CreateNewPanel()
    {
        currentHighlightPanel = Instantiate(CultureHighlightPanelPefab, Vector3.zero, Quaternion.identity) as GameObject;
        CultureHighlightPanel chp = currentHighlightPanel.GetComponent<CultureHighlightPanel>();
        chp.Init(gameObject);
    }

    void DestroyPanel()
    {
        Destroy(currentHighlightPanel);
    }
}
