using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CultureMemory : MonoBehaviour
{
    public Culture Culture;

<<<<<<< HEAD

=======
    [SerializeField]
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
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


<<<<<<< HEAD
    private void Awake()
=======
    private void Start()
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    {
        Culture.OnNameChanged += CultureMemory_OnNameChanged;
    }

    public void CultureMemory_OnNameChanged(object sender, Culture.OnCultureNameChangedEventArgs e)
    {
        cultureParentName = e.OldName;
    }


}
