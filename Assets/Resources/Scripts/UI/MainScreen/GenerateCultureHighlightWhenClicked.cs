using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCultureHighlightWhenClicked : MonoBehaviour
{
    public GameObject cultureHighlighterObj;

    private void OnMouseUp()
    {
        GameObject.Instantiate(cultureHighlighterObj);
        cultureHighlighterObj.GetComponent<CultureHighlighter>().SetPosition(transform);
    }
}
