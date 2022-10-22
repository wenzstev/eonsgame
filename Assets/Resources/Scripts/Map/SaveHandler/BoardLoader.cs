using System.Collections;
using UnityEngine;


public class BoardLoader : MonoBehaviour
{
    public GameObject BoardTemplate;

    GameObject BoardObj;

    public GameObject LoadBoardFromSerialized(SerializedBoard sb)
    {
        BoardObj = Instantiate(BoardTemplate);
        BoardStats bs = BoardObj.GetComponent<BoardStats>();
        bs.height = sb.height;
        bs.width = sb.width;
        return BoardObj;
    }

}
