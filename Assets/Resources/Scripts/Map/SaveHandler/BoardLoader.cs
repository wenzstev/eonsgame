using System.Collections;
using UnityEngine;


public class BoardLoader : MonoBehaviour
{
    public GameObject BoardTemplate;

    GameObject BoardObj;

    public GameObject LoadBoardFromSerialized(SerializedBoard sb)
    {
        BoardObj = Instantiate(BoardTemplate);
        JsonUtility.FromJsonOverwrite(sb.serializedComponents[0], BoardObj.GetComponent<BoardStats>());
        return BoardObj;
    }

}
