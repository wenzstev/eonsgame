using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCultureHighlightWhenClicked : MonoBehaviour
{
    public GameObject CultureHighlightPanelPefab;

    private void OnMouseUp() // TODO: change this to an event so that it can be combined with the main system
    {
        GameObject cultureHighlighterObj = Instantiate(CultureHighlightPanelPefab, Vector3.zero, Quaternion.identity) as GameObject;
        CultureHighlightPanel chp = cultureHighlighterObj.GetComponent<CultureHighlightPanel>();
        chp.Init(gameObject);
    }
}
