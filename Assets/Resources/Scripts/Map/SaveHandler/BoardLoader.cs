using System;
using System.Collections;
using UnityEngine;


public class BoardLoader : MonoBehaviour
{
    public GameObject BoardTemplate;

    GameObject BoardObj;

    public event EventHandler<OnBoardCreatedEventArgs> OnBoardCreated;


    public GameObject LoadBoardFromSerialized(SerializedBoard sb)
    {
        BoardObj = Instantiate(BoardTemplate);
        JsonUtility.FromJsonOverwrite(sb.serializedComponents[0], BoardObj.GetComponent<BoardStats>());
        BoardObj.GetComponent<BoardStats>().Initialize();
        OnBoardCreated?.Invoke(this, new OnBoardCreatedEventArgs() { BoardStats = BoardObj.GetComponent<BoardStats>() });
        return BoardObj;
    }

    public class OnBoardCreatedEventArgs : EventArgs
    {
        public BoardStats BoardStats;
    }
}
