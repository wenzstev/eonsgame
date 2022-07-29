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
        SceneManager.LoadScene("Title Screen");
    }

    public void LoadLoadMapScene()
    {
        SceneManager.LoadScene("Load Map");
    }
}
