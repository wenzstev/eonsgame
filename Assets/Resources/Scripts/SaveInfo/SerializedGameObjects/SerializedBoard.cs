using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SerializedBoard : SerializedGameObject
{
    public SerializedBoard(Board b)
    {
        GameObject boardObj = b.gameObject;
        serializedComponents.Add(JsonUtility.ToJson(boardObj.GetComponent<BoardStats>()));
    }
}