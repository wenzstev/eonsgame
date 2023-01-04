using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private static TimeController timeController;

    public static TimeController instance
    {
        get
        {
            if (!timeController)
            {
                timeController = FindObjectOfType(typeof(TimeController)) as TimeController;

                if (!timeController)
                {
                    Debug.LogError("There needs to be one active TimeController script on a GameObject in your scene.");
                }
            }
            return timeController;
        }
    }

    public float[] speeds;

    int curSpeedLevel;

    float curSpeed;
    float timer = 0;

    bool isPaused = true;

    public static int GetCurTimeLevel()
    {
        return instance.curSpeedLevel;
    }

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
        curSpeedLevel = newSpeedLevel;
    }

    void TogglePause(Dictionary<string, object> empty)
    {
        isPaused = !isPaused;
    }



}
