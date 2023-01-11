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

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        image.color = startColor;
        yield return new WaitForSeconds(startPause);
        float curTime = 0;
        while (curTime < fadeTime)
        {
            curTime += Time.deltaTime;
            float curLerp = Mathf.InverseLerp(0, fadeTime, curTime);
            float curAlpha = Mathf.Lerp(1, 0, curLerp);
            image.color = new Color(startColor.r, startColor.g, startColor.b, curAlpha);
            yield return null;
        }
        Destroy(gameObject);
    }
}
