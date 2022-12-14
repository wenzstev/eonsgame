using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapController : MonoBehaviour
{
    // poll intermittently for board
    // if can find board
    //      get edge coordinates from board 
    //      trigger event with edge coordinates in it

    Board board;

    public event OnBoardCreated<OnBoardCreatedEventArgs>();

    private void Awake()
    {
        StartCoroutine(PollForBoard());
    }

    public IEnumerator PollForBoard()
    {
        while(board == null)
        {
            Debug.Log("Checking for board...");
            board = FindObjectOfType<Board>();
            yield return new WaitForSeconds(.1f);
        }

    }

    public class OnBoardCreatedEventArgs : EventArgs
    {

    }

}
