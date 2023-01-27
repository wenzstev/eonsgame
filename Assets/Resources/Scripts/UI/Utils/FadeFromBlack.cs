using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFromBlack : MonoBehaviour
{
    public Image image;

    public Color startColor;
    public float fadeTime;
    public float startPause = 0;

    public EventHandler<EventArgs> OnFadeComplete;


    public void FadeIn()
    {
        StartCoroutine(FadeCoroutine(false));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(true));

    }

    IEnumerator FadeCoroutine(bool fadeOut)
    {
        float alphaStart = fadeOut ? 1 : 0;
        float alphaEnd = fadeOut ? 0 : 1;
  

        image.color = new Color(startColor.r, startColor.g, startColor.b, alphaStart);
        yield return new WaitForSeconds(startPause);
        float curTime = 0;
        while (curTime < fadeTime)
        {
            curTime += Time.deltaTime;
            float curLerp = Mathf.InverseLerp(0, fadeTime, curTime);
            float curAlpha = Mathf.Lerp(alphaStart, alphaEnd, curLerp);
            image.color = new Color(startColor.r, startColor.g, startColor.b, curAlpha);
            yield return null;
        }

        OnFadeComplete?.Invoke(this, EventArgs.Empty);

    }

}
