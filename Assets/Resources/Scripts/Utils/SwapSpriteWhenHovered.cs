using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwapSpriteWhenHovered : MonoBehaviour
{
    Sprite unHovered;
    public Sprite hovered;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        unHovered = sr.sprite;
    }

    private void OnMouseEnter()
    {
        sr.sprite = hovered;
    }

    private void OnMouseExit()
    {
        sr.sprite = unHovered;
    }

}
