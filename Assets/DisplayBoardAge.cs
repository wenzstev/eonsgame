using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayBoardAge : MonoBehaviour
{
    int Age;
    TextMeshProUGUI Text;
    bool HasAge = false;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("NewBoardAge", SetNewBoardAge);
        Text = GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(GetCurrentBoardAge());
    }

    IEnumerator GetCurrentBoardAge()
    {
        while (!HasAge)
        {
            EventManager.TriggerEvent("RequestBoardAge", null);
            yield return null;
        }
    }

    void SetNewBoardAge(Dictionary<string, object> newAge)
    {
        HasAge = true;
        Age = (int)newAge["age"];
        Text.text = convertAgeToTime(Age);
    }

    string convertAgeToTime(int age)
    {
        int years = age / 360;
        int daysLeftInYear = age % 360;
        int months = daysLeftInYear / 30;
        int daysLeftInMonth = daysLeftInYear % 30;

        return $"{years} / {months} / {daysLeftInMonth}"; // creates some garbage, unsure how to fix
    }

    private void OnDestroy()
    {
        EventManager.StopListening("NewBoardAge", SetNewBoardAge);
    }
}
