using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESC : MonoBehaviour
{
    public GameObject gameObjectEsc;

    private void Start()
    {
        gameObjectEsc = GameObject.Find("UIEscape").transform.Find("UIESC").gameObject;
    }

    public void ClickResume()
    {
        Debug.Log("Escape: Click resume");
        gameObjectEsc.SetActive(false);
        Time.timeScale = 1;
    }

    public void ClickHomePage()
    {
        Debug.Log("Escape: Click home");
        // TODO Print unsaved status to confirm twice
        SceneManager.LoadScene("Scenes/Menu", LoadSceneMode.Single);
    }

    public void ClickRestart()
    {
        Debug.Log("Escape: Click restart");
        // Go to check point
        GameObject.FindWithTag("Player").transform.position =
            GameObject.FindWithTag("Player").GetComponent<PlayerStatus>().courseStart;

        // Remove all spawn bricks
        var bricks = GameObject.FindGameObjectsWithTag("Spawn");
        foreach (var brick in bricks)
        {
            Destroy(brick);
        }

        // Reset button
        var buttons = GameObject.FindGameObjectsWithTag("BrickButton");
        foreach (var button in buttons)
        {
            button.GetComponent<BrickInteraction>().brickNumber = button.GetComponent<BrickInteraction>().brickInit;
            button.GetComponent<BrickInteraction>().remainTextBox.text =
                button.GetComponent<BrickInteraction>().brickNumber.ToString();
        }

        gameObjectEsc.SetActive(false);
        Time.timeScale = 1;

        // Data count
        GameObject.FindWithTag("Player").GetComponent<PlayerStatus>().cntRestart++;
    }
}