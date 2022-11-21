using System.Collections;
using UnityEngine;
using TMPro;


public class Indicator : MonoBehaviour
{

    public float AnimationTime;
    public float YMoveAmount;
    TextMeshPro _indicatorText;

    public void Initialize()
    {
        _indicatorText = GetComponent<TextMeshPro>();
        StartCoroutine(PlayAnimation());
    }

    // TODO: you have enough of these lerp/inverselerp coroutines to split them out and make them their own class
    public IEnumerator PlayAnimation()
    {
        float timer = 0;
        float StartPositionY = transform.position.y;
        float EndPositionY = StartPositionY + YMoveAmount;
        float StartPositionX = transform.position.x;
        while (timer < AnimationTime)
        {
            float CurInverseLerpPoint = Mathf.InverseLerp(0, AnimationTime, timer);
            transform.position = new Vector2(StartPositionX, Mathf.Lerp(StartPositionY, EndPositionY, CurInverseLerpPoint));
            _indicatorText.color = FadeByLerp(CurInverseLerpPoint, _indicatorText.color);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    public Color FadeByLerp(float lerp, Color color)
    {
        return new Color(color.r, color.g, color.b, Mathf.Lerp(1, 0, lerp));
    }

}
