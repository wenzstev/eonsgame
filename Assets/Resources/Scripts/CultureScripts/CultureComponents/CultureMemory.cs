using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CultureMemory : MonoBehaviour
{
    public Culture Culture;

    [SerializeField]
    Tile _previousTile;
    public Tile previousTile { 
        get => _previousTile;
        set 
        {
            if(value != null)
            {
                _previousTile = value;
            }
        } 
    }

    public string cultureParentName;

    [SerializeField]
    Culture.State _previousState;

    public Culture.State previousState
    {
        get => _previousState;
        set
        {
            if(value == Culture.State.Repelled)
            {
                wasRepelled = true;
            }
            _previousState = value;
        }
    }

    public bool wasRepelled;


    private void Start()
    {
        Culture.OnNameChanged += CultureMemory_OnNameChanged;
    }

    public void CultureMemory_OnNameChanged(object sender, Culture.OnCultureNameChangedEventArgs e)
    {
        cultureParentName = e.OldName;
    }


}
