using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOnStart : MonoBehaviour
{
    public FadeFromBlack Fader;

    // Start is called before the first frame update
    void Start()
    {
        Fader.FadeOut();
    }

}
