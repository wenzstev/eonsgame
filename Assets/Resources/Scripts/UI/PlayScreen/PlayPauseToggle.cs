using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPauseToggle : MonoBehaviour
{
    public Sprite playSprite;
    public Sprite pauseSprite;

    Image image;

    bool isPaused = true;


    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = pauseSprite;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        image.sprite = isPaused ? pauseSprite : playSprite;
        EventManager.TriggerEvent("PauseSpeed", null);
    }


}
