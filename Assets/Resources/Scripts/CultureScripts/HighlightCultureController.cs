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
        EventManager.StartListening("HoverCulture" + culture.name, HighlightCulture);
        Debug.Log("initializing hover for " + culture.name);
        sr.color = offHighlight;
    }


    void HighlightCulture(Dictionary<string, object> empty)
    {
        sr.color = culture.color;
        Debug.Log("highlighting " + culture.name);

        //EventManager.StartListening("HoverOff", RemoveHighlight);
        EventManager.StopListening("HoverCulture" + culture.name, HighlightCulture);

    }

  
}
