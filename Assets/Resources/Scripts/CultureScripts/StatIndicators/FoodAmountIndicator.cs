using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodAmountIndicator : MonoBehaviour
{
    TextMeshPro _amountText;
    public float Amount;
    public Color PositiveColor;
    public Color NegativeColor;

    public void Initialize(float amount)
    {
        _amountText = GetComponent<TextMeshPro>();
        Amount = amount;
        _amountText.color = Amount < 0 ? NegativeColor : PositiveColor;
        _amountText.text = Mathf.FloorToInt(Amount).ToString();
        GetComponent<Indicator>().Initialize();
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


