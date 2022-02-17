using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public GameObject gameObjectEsc;

    private void Start()
    {
        gameObjectEsc = GameObject.Find("UIPublic").transform.Find("UIESC").gameObject;
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