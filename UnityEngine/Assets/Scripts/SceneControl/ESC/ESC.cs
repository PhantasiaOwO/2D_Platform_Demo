using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESC : MonoBehaviour
{
    public GameObject gameObjectEsc;

    public void ClickResume()
    {
        gameObjectEsc.SetActive(false);
        Time.timeScale = 1;
    }

    public void ClickHomePage()
    {
        // TODO Print unsaved status
        SceneManager.LoadScene("Scenes/Menu");
    }

    public void ClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}