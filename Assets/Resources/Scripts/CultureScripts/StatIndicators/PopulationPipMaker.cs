using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PopulationPipMaker : MonoBehaviour
{
    public GameObject PopPip;
    Culture culture;
    public int MaxPips;
    public float DegreeSeparation;
    public float Radius;

    int[] PipLevels = { 5, 10, 20, 50, 100, 500, 1000 };

    private void Start()
    {
        culture = GetComponentInParent<Culture>();
        EventManager.StartListening("Tick", CheckPopulation);
        SetPips(culture.Population);
    }

    void CheckPopulation(Dictionary<string, object> empty) // thought: to improve performance, maybe do some of these calculations on different frames than the tick?
    {
        int curPop = culture.Population;
        SetPips(curPop);
        
    }

    void SetPips(int pop)
    {

        int desiredLevel = 0;
        while (PipLevels[desiredLevel] <= pop && desiredLevel < PipLevels.Length) desiredLevel++;
        int curLevel = transform.childCount;

        int emergencyCount = 0;
        while(curLevel != desiredLevel & emergencyCount < 50)
        {
            curLevel = transform.childCount;
            if (desiredLevel < curLevel) RemovePip();
            if (desiredLevel > curLevel) AddPip();
            emergencyCount++;
        }

        if (emergencyCount == 50) Debug.LogError($"Emergency count triggered! desiredLevel: {desiredLevel} children: {String.Join(", ", GetChildren())} population: {pop}");

    }

    List<GameObject> GetChildren()
    {
        List<GameObject> children = new List<GameObject>();
        for(int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
        return children;
    }

    void AddPip()
    {
        bool willBeEven = (transform.childCount + 1) % 2 == 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            PopulationPip curPip = transform.GetChild(i).GetComponent<PopulationPip>();
            curPip.IncrementAngle();
        }

        GameObject newPip = Instantiate(PopPip, gameObject.transform);
        newPip.GetComponent<PopulationPip>().Init(willBeEven, DegreeSeparation, Radius);

        if (willBeEven) newPip.GetComponent<PopulationPip>().IncrementAngle();
    }

    void RemovePip()
    {
        if (transform.childCount == 0) return;
        GameObject LatestPip = transform.GetChild(transform.childCount-1).gameObject;
        LatestPip.transform.SetParent(null); // have to remove as child, destroy won't run until the end of the frame
        Destroy(LatestPip);
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<PopulationPip>().DecrementAngle();
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening("Tick", CheckPopulation);
    }

}
