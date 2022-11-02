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

    private void Start()
    {
        EventManager.StartListening("Tick", IncrementBoardAge);
        EventManager.StartListening("RequestBoardAge", ProvideBoardAge);
    }

    void SendBoardAge()
    {
        EventManager.TriggerEvent("NewBoardAge", new Dictionary<string, object> { { "age", BoardStats.Age } });
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
