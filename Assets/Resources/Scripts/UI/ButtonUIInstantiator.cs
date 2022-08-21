using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUIInstantiator : MonoBehaviour
{
    public void CreateObj(GameObject obj)
    {
        GameObject canvas = FindObjectOfType<Canvas>().gameObject;
        GameObject objInstance = Instantiate(obj, canvas.transform);
    }
}
