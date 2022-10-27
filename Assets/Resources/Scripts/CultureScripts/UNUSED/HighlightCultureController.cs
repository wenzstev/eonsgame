using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCultureController : MonoBehaviour
{
    SpriteRenderer sr;
    Culture culture;

    public Color offHighlight;

    


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        culture = GetComponentInParent<Culture>();

    }

    void OnEnable()
    {
        //Debug.Log("enabling " + culture.name);
        sr.color = offHighlight;
        sr.sortingOrder = 0;
        EventManager.StartListening("HoverCulture" + culture.name, HighlightCulture);
    }






    void HighlightCulture(Dictionary<string, object> empty)
    {
        if(this == null)
        {
            return; // temporary way to prevent destroyed objects from messing up the invoke
        }
        sr.color = culture.Color;
        sr.sortingOrder = 5;

        //EventManager.StartListening("HoverOff", RemoveHighlight);
        //EventManager.StopListening("HoverCulture" + culture.name, HighlightCulture);

    }

    private void OnDisable()
    {
        EventManager.StopListening("HoverCulture" + culture.name, HighlightCulture);
    }


}
