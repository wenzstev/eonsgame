using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CultureInfoPanelHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    CultureAggregation aggregation;

    void Start()
    {
        aggregation = GetComponent<CultureAggregateInfo>().cultureAggregate;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.TriggerEvent("ChangeViewMode", new Dictionary<string, object> { { "view", ViewController.Views.Highlight } });
        EventManager.TriggerEvent("HoverCulture" + aggregation.name, null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.TriggerEvent("HoverOff", null);
    }
}
