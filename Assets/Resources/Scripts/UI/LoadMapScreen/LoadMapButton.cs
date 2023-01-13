using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;
using System.IO;

public class LoadMapButton : MonoBehaviour
{
    TMP_Text saveName;
    string filePath;
    string fileName;


    public void Init(string filePath)
    {
        this.filePath = filePath;
        saveName = GetComponentInChildren<TMP_Text>();
        fileName = Path.GetFileNameWithoutExtension(filePath);
        if(fileName != "")
        {
            saveName.text = fileName;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMap()
    {
        Save save = Save.UnserializeSave(fileName);
        Save.CreatePersistantSave(save, fileName);
        SceneManager.LoadScene("PlayScene");
    }

    public void DeleteSave()
    {
        File.Delete(filePath);
        Destroy(gameObject);
    }
}
