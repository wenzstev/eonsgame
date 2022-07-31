using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    public Save save { get; private set; }

    public void InitializeSave(Save s)
    {
        save = s;
        DontDestroyOnLoad(gameObject);
    }

}
