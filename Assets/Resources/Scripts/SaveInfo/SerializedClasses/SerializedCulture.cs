using System.Collections;
using UnityEngine;

[System.Serializable]
public class SerializedCulture
{
    public string name;

    public SerializedColor color;
    public SerializedCultureMemory cultureMemory;

    public int population;

    public int affinity;

    public int currentState;

    public SerializedCulture(Culture culture)
    {
        name = culture.name;
        color = new SerializedColor(culture.color);
        population = culture.population;
        affinity = (int)culture.affinity;
        currentState = (int)culture.currentState;
    }

}