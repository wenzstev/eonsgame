using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInstantiator : MonoBehaviour
{
    public void CreateObj(GameObject obj)
    {
        GameObject canvas = FindObjectOfType<Canvas>().gameObject;
        GameObject objInstance = Instantiate(obj, canvas.transform);
        objInstance.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);

    }
}
