using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float[] speeds;

    float curSpeed;
    float timer = 0;

    bool isPaused = true;

    /*
     Level 1: 1 tick per second => 1
     Level 2: 2 ticks per second => .5f
     Level 3: 4 ticks per second => .25f
     Level 4: 8 ticks per second => .125f
     */


    private void Start()
    {
        EventManager.StartListening("PauseSpeed", TogglePause);
        EventManager.StartListening("SpeedChange", ChangeSpeed);

        curSpeed = speeds[0];
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
        if (timer >= curSpeed)
        {
            EventManager.TriggerEvent("Tick", null);
            timer = 0;
        }
    }

    void ChangeSpeed(Dictionary<string, object> newSpeedDict)
    {
        int newSpeedLevel = (int)newSpeedDict["speed"];
        if(newSpeedLevel < 0 || newSpeedLevel >= speeds.Length)
        {
            Debug.LogWarning("Tried to change to nonexistant speed!");
            return;
        }

        curSpeed = speeds[newSpeedLevel];
    }


    void TogglePause(Dictionary<string, object> empty)
    {
        isPaused = !isPaused;
    }



}
