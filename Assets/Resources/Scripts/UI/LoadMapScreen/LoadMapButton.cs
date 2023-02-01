using System;
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

    public FadeFromBlack Fader;

    private void Awake()
    {
        Fader.OnFadeComplete += Fader_OnFadeComplete;
    }

    void Fader_OnFadeComplete(object sender, EventArgs e)
    {
        LoadMap();
    }

    public void Init(string filePath)
    {
        this.filePath = filePath;
        saveName = GetComponentInChildren<TMP_Text>();
        fileName = Path.GetFileNameWithoutExtension(filePath);
        if(fileName == "" || Path.GetExtension(filePath) != ".json")
        {
            Destroy(gameObject);
        }
        else
        {
            saveName.text = fileName;
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
