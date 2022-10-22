using System.Collections;
using UnityEngine;


public class LoadBoard : MonoBehaviour
{
    public GameObject BoardTemplate;

    GameObject BoardObj;

    public void Start()
    {
        BoardObj = Instantiate(BoardTemplate);
        LoadBoardFromSerialized();
    }

    public void LoadBoardFromSerialized()
    {

    }

}
