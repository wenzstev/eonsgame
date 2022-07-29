using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardProvider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("BoardRequested", OnBoardRequested);
    }

    void OnBoardRequested(Dictionary<string, object> empty)
    {
        Board b = GetComponent<Board>();
        EventManager.TriggerEvent("BoardProvided", new Dictionary<string, object> { {"board", b } });
    }
}
