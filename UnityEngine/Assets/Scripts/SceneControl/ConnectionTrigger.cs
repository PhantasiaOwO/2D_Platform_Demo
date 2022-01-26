using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionTrigger : MonoBehaviour
{
    public GameObject player;

    private bool isTriggered;

    private void Start()
    {
        isTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (isTriggered) return;

        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(LoadSceneAsync());
        isTriggered = true;
    }

    IEnumerator LoadSceneAsync()
    {
        var thisScene = SceneManager.GetActiveScene().buildIndex;
        var loadStatus = SceneManager.LoadSceneAsync(thisScene + 1, LoadSceneMode.Additive);

        while (!loadStatus.isDone)
        {
            yield return null;
        }

        // Set same start of every scene
        player.transform.position = new Vector3(0, 0, 1);

        // TODO Move vital game object to new scene
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByBuildIndex(thisScene + 1));

        SceneManager.UnloadSceneAsync(thisScene);
    }
}