using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControllerUI : MonoBehaviour
{
    public void PauseSpeed()
    {
        EventManager.TriggerEvent("PauseSpeed", null);
    }

    public void NormalSpeed()
    {
        EventManager.TriggerEvent("NormalSpeed", null);
    }

    public void FastSpeed()
    {
        EventManager.TriggerEvent("FastSpeed", null);
    }
}
