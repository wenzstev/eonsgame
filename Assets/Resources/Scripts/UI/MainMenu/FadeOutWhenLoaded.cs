using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutWhenLoaded : MonoBehaviour
{
    public FadeFromBlack Fader;

    private void Awake()
    {
        Fader.image = GameObject.Find("FadeFromBlack").GetComponent<FadeFromBlack>().image;
    }

    public void LoadMap()
    {
        Fader.FadeIn();
    }
}
