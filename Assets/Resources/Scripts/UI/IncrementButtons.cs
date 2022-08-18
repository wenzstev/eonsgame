using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementButtons : MonoBehaviour
{
    public void IncreaseSpeed()
    {
        EventManager.TriggerEvent("DialRotated", new Dictionary<string, object> { {"direction", 1 } });
    }

    public void DecreaseSpeed()
    {
        EventManager.TriggerEvent("DialRotated", new Dictionary<string, object> { { "direction", -1 } });
    }

    public void SetDial(int newIndex)
    {
        EventManager.TriggerEvent("DialSetTo", new Dictionary<string, object> { { "index", newIndex } });

    }
}
