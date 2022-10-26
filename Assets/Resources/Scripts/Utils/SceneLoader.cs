using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNewMapScene()
    {
        SceneManager.LoadScene("NewMapScene");
    }

    public void LoadMainMenuScene()
    {
        EventManager.TriggerEvent("LoadingMainMenu", null);
        SceneManager.LoadScene("Title Screen");
    }

    public void LoadLoadMapScene()
    {
        SceneManager.LoadScene("Load Screen");
    }
}
