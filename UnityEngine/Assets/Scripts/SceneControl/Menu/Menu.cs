using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public void ClickNewGame()
    {
        Debug.Log("Click NewGame");
        SceneManager.LoadScene("Scenes/001");
    }


    public void ClickSaveList()
    {
        // TODO Load Save list scene
    }

    public void ClickExit()
    {
        Debug.Log("Click Exit");
        Application.Quit();
    }
}