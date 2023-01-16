using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class AffinityDisplay : MonoBehaviour
{
    public TextMeshProUGUI TopBiome;
    AffinityManager AffinityManager;

    public void SetValues(Culture c)
    {
        AffinityManager = c.GetComponent<AffinityManager>();
        SetHighestAffinity();
    }

    void SetHighestAffinity()
    {
        (TileDrawer.BiomeType, float) topAffinity = AffinityManager.GetAllAffinities()
            .OrderByDescending(v => v.Item2).First();
        TopBiome.text = DisplayUtils.SplitCamelCase(topAffinity.Item1.ToString());
    }

    void AffinityManager_OnAffinityChanged(object sender, AffinityManager.OnAffinityChangedEventArgs e)
    {
        SetHighestAffinity();
    }
}
