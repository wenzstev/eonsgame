using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public int normalSpeed;
    public int fastSpeed;

    float timeBetweenTicks;
    float timer = 0;

    bool isPaused = true;


    private void Start()
    {
        EventManager.StartListening("PauseSpeed", TogglePause);
        EventManager.StartListening("NormalSpeed", SetSpeedNormal);
        EventManager.StartListening("FastSpeed", SetSpeedFast);

        timeBetweenTicks = normalSpeed;
    }


    private void Update()
    {
        if (!isPaused)
        {
            CalculateTimeToTick();
        }
    }

    void CalculateTimeToTick()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenTicks)
        {
            EventManager.TriggerEvent("Tick", null);
            timer = 0;
        }
    }

    void ChangeSpeed(int newSpeed)
    {
        timeBetweenTicks = 1 / (float) newSpeed;
        isPaused = false;
    }


    void SetSpeedNormal(Dictionary<string, object> empty)
    {
        ChangeSpeed(normalSpeed);
    }

    void SetSpeedFast(Dictionary<string, object> empty)
    {
        ChangeSpeed(fastSpeed);
    }

    void TogglePause(Dictionary<string, object> empty)
    {
        isPaused = !isPaused;
    }



}
