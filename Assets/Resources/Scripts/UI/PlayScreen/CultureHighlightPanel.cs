using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CultureHighlightPanel : MonoBehaviour
{
    RectTransform rectTransform;
    GameObject cultureObj;
    public void Init(GameObject cultureToTarget)
    {
        cultureObj = cultureToTarget;
        Canvas canvas = FindObjectsOfType<Canvas>()[0];
        transform.SetParent(canvas.transform);

        rectTransform = GetComponent<RectTransform>();
        SetPanelToCultureInfo(cultureObj.GetComponent<Culture>());
    }

    IEnumerator StartListeningForClickAway()
    {
        yield return new WaitForSeconds(.2f);
        EventManager.StartListening("InteractiveMouseUp", OnInteractiveMouseUp);
    }

    void Update()
    {
        rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(cultureObj.transform.position);
    }

    void OnInteractiveMouseUp(Dictionary<string, object> mouseButton)
    {
        EventManager.StopListening("MouseUpGeneric", OnInteractiveMouseUp);
        Destroy(gameObject);
    }


    void SetPanelToCultureInfo(Culture c)
    {
        GameObject colorPill = transform.GetChild(0).gameObject;
        colorPill.GetComponent<Image>().color = c.Color;
        GameObject cultureName = transform.GetChild(1).gameObject;
        cultureName.GetComponent<TextMeshProUGUI>().text = c.name;
        GameObject popText = transform.GetChild(2).gameObject;
        popText.GetComponent<TextMeshProUGUI>().text = c.Population.ToString();
    }
}
