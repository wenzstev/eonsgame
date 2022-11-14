using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodAmountIndicator : MonoBehaviour
{
    TextMeshPro _amountText;
    float _amount;
    public Color PositiveColor;
    public Color NegativeColor;

    public float AnimationTime;
    public float YMoveAmount;

    public void Initialize(float amount)
    {
        _amountText = GetComponent<TextMeshPro>();
        _amount = amount;
        _amountText.color = _amount < 0 ? NegativeColor : PositiveColor;
        _amountText.text = Mathf.FloorToInt(_amount).ToString();
        StartCoroutine(PlayAnimation());
    }


    // TODO: you have enough of these lerp/inverselerp coroutines to split them out and make them their own class
    public IEnumerator PlayAnimation()
    {
        float timer = 0;
        float StartPositionY = transform.position.y;
        float EndPositionY = StartPositionY + YMoveAmount;
        float StartPositionX = transform.position.x;
        while(timer < AnimationTime)
        {
            float CurInverseLerpPoint = Mathf.InverseLerp(0, AnimationTime, timer);
            transform.position = new Vector2(StartPositionX, Mathf.Lerp(StartPositionY, EndPositionY, CurInverseLerpPoint));
            _amountText.color = FadeByLerp(CurInverseLerpPoint, _amountText.color);
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


// rump sketch of what a lerp mover class might look like
public abstract class InverseLerpMover : MonoBehaviour
{
    float Counter;
    float AnimationTime;
    List<float> StartValues;
    List<float> EndValues;
}


