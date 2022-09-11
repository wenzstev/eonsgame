using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPickerSystem : MonoBehaviour
{

    public GameObject selectIndicator;
    GameObject _selectIndicator;

    public GameObject[] Options;

    int curOption = 0;
    public string pickerID;

    private void Start()
    {
        _selectIndicator = Instantiate(selectIndicator);
        SetIndicator(curOption);
    }

    public void SetIndicator(int newOption)
    {
        _selectIndicator.transform.parent = Options[newOption].transform;
        _selectIndicator.transform.localPosition = Vector3.zero;

        curOption = newOption;

        EventManager.TriggerEvent("PickerOptionSelected", new Dictionary<string, object> { {"pickerID", pickerID}, {"option", curOption} });

    }

}
