using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;

public class LoadInfo : MonoBehaviour
{
    TMP_Text saveName;
    string filePath;


    public void Init(string filePath)
    {
        this.filePath = filePath;
        saveName = GetComponentInChildren<TMP_Text>();
        string potentialFileName = GetFilenameFromPath(filePath);
        if(potentialFileName != "")
        {
            saveName.text = potentialFileName;
        }
        else
        {
            Destroy(gameObject);
        }



    }

    public void LoadMap()
    {
        Save save = Save.UnserializeSave(filePath);
        Save.CreatePersistantSave(save);
        SceneManager.LoadScene("PlayScene");
    }

    public static string GetFilenameFromPath(string filePath)
    {
        //Debug.Log(filePath);
        Regex rx = new Regex(@"\\(.*).json");
        //Debug.Log(rx);
        //Debug.Log(rx.Match(filePath));
        Match match = rx.Match(filePath);
        if(match.Success)
        {
            return match.Groups[1].Value;
        }
        return "";
    }



}
