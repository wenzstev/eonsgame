using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public bool isOpen = false;



    public void toggleOpen()
    {
        isOpen = !isOpen;
        gameObject.SetActive(isOpen);
    }
}
