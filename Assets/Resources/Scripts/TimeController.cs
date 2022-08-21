using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public int[] speeds;

    float timeBetweenTicks;
    float timer = 0;

    bool isPaused = true;


    private void Start()
    {
        EventManager.StartListening("PauseSpeed", TogglePause);
        EventManager.StartListening("SpeedChange", ChangeSpeed);

        timeBetweenTicks = 1 / (float) speeds[0];
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

    void ChangeSpeed(Dictionary<string, object> newSpeedDict)
    {
        int newSpeedLevel = (int)newSpeedDict["speed"];
        if(newSpeedLevel < 0 || newSpeedLevel >= speeds.Length)
        {
            Debug.LogWarning("Tried to change to nonexistant speed!");
            return;
        }

        int newSpeed = speeds[newSpeedLevel];

        timeBetweenTicks = 1 / (float) newSpeed;
    }


    void TogglePause(Dictionary<string, object> empty)
    {
        isPaused = !isPaused;
    }



}
