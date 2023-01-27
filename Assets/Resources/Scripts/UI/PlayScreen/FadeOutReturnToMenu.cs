using System;
using UnityEngine;

public class FadeOutReturnToMenu : MonoBehaviour
{
    FadeFromBlack fader;

    public SceneLoader sceneLoader;

    private void Awake()
    {
        fader = GameObject.Find("Fader").GetComponent<FadeFromBlack>();
        fader.OnFadeComplete += Fader_OnFadeComplete;


    }

    public void ReturnToMenu()
    {
        fader.FadeIn();
    }

    void Fader_OnFadeComplete(object sender, EventArgs e)
    {
        sceneLoader.LoadMainMenuScene();
    }

}
