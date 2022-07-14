using System;
using System.Collections;
using System.Collections.Generic;
using Unity;

[Serializable]
public class CultureAttributes 
{
    public string name { get; private set; }
    public SerializedColor color { get; private set; }
    public string tile { get; private set; }

    public CultureMemory cultureMemory { get; private set; }

    public int population { get; private set; }

    public float spreadChance = 1f;

    public float shareCultureChance = .05f;

    public float baseMutationMax = .01f;

    public float sameCultureCutoff = .1f;

    public float growPopulationChance = .01f;

    public float gainAffinityChance = .003f;

    public int maxPopTransfer = 1;

    public float influenceRate = .05f;

    public string affinity { get; private set; }

    public int maxOnTile;

    public string currentState;
}

[Serializable]
public class SerializedColor
{
    public float r;
    public float g;
    public float b;
}