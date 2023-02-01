using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CultureMemory : MonoBehaviour
{
    Culture Culture;

    Tile _previousTile;
    public Tile previousTile { 
        get 
        {
            return _previousTile;
        }
        set 
        {
            if(value != null)
            {
                _previousTile = value;
            }
        } 
    }

    public string cultureParentName;

    public int daysSinceEaten = 0;

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


    private void Awake()
    {
        if (Culture == null) Culture = GetComponent<Culture>();
        Culture.OnNameChanged += CultureMemory_OnNameChanged;
    }

    public void CultureMemory_OnNameChanged(object sender, Culture.OnCultureNameChangedEventArgs e)
    {
        cultureParentName = e.OldName;
    }


}
