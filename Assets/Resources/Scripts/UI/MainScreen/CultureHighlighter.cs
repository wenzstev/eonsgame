using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureHighlighter : MonoBehaviour
{
    public GameObject parentUIObj;

    public void SetPosition(Transform culturePosition)
    {
        transform.SetParent(parentUIObj.transform);
    }
}
