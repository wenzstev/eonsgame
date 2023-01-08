using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AffinityBarController : MonoBehaviour
{
    public TextMeshProUGUI AffinityName;
    public RectTransform AffinityBar;

    const float MIN_WIDTH = 6f;
    const float MAX_WIDTH = 280f;

    public void SetValues(TileDrawer.BiomeType biome, float affinity, float MaxAffinityAmount)
    {
        AffinityName.text = biome.ToString();

        float barPercent = Mathf.InverseLerp(0, MaxAffinityAmount, affinity);
        float barWidth = Mathf.Lerp(MIN_WIDTH, MAX_WIDTH, barPercent);

        float barHeight = AffinityBar.sizeDelta.y;
        AffinityBar.sizeDelta = new Vector2(barWidth, barHeight);
    }
}
