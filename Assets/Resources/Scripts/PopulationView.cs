using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationView : MonoBehaviour
{
    Culture culture;
    SpriteRenderer sr;

    public Color lowColor;
    public Color moderateColor;
    public Color highColor;

    public int midPopulationValue;
    public int highPopulationValue;

    private void Awake()
    {
        culture = transform.parent.gameObject.GetComponent<Culture>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("Tick", OnTick);
    }

    private void OnTick(Dictionary<string, object> empty)
    {
        int curPop = culture.population;
        if (curPop < midPopulationValue)
        {
            float position = Mathf.InverseLerp(0, (float)midPopulationValue, curPop);
            sr.color = Color.Lerp(lowColor, moderateColor, position);
        }
        else
        {
            float position = Mathf.InverseLerp(midPopulationValue, highPopulationValue, curPop);
            sr.color = Color.Lerp(moderateColor, highColor, position);
        }
    }

    private void OnDisable()
    {
        EventManager.StopListening("Tick", OnTick);
    }

}
