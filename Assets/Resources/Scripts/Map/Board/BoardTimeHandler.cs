using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTimeHandler : MonoBehaviour
{

    BoardStats boardStats;
    BoardStats BoardStats
    {
        get
        {
            if (boardStats == null) boardStats = GetComponent<BoardStats>();
            return boardStats;
        }
    }

    Dictionary<string, object> boardStatsAgeDict;


    private void Start()
    {
        EventManager.StartListening("Tick", IncrementBoardAge);
        EventManager.StartListening("RequestBoardAge", ProvideBoardAge);
        boardStatsAgeDict = new Dictionary<string, object>() { { "age", 0 } };
    }

    void SendBoardAge()
    {
        boardStatsAgeDict["age"] = BoardStats.Age;
        EventManager.TriggerEvent("NewBoardAge", boardStatsAgeDict);
    }

    void IncrementBoardAge(Dictionary<string, object> empty)
    {
        BoardStats.Age++;
        SendBoardAge();
    }

    void ProvideBoardAge(Dictionary<string, object> empty)
    {
        SendBoardAge();
    }

    private void OnDestroy()
    {
        EventManager.StopListening("Tick", IncrementBoardAge);
        EventManager.StopListening("RequestBoardAge", ProvideBoardAge);

    }
}
