using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTrigger : MonoBehaviour
{
    public GameObject finishCanvas;

    private void Start()
    {
        finishCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        finishCanvas.SetActive(true);
        // Animator will auto run
        finishCanvas.GetComponent<DataFill>().RefreshData();

        var playerObj = GameObject.FindWithTag("Player");
        // Lock player control
        playerObj.GetComponent<Control>().canMove = false;

        // Menu-ContinueButton disabled
        playerObj.GetComponent<PlayerStatus>().DeletePlayerStatusFile();
    }
}