using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadList : MonoBehaviour
{
    public GameObject LoadPrefab;

    // Start is called before the first frame update
    void Start()
    {
        string[] fileNames = Directory.GetFiles(Application.persistentDataPath);

        foreach(string name in fileNames)
        {
            GameObject curLoad = Instantiate(LoadPrefab);
            curLoad.transform.SetParent(transform);
            curLoad.GetComponent<LoadInfo>().Init(name);
        }
    }


}
