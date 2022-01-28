using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Unload scene
        var thisSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("This scene is " + thisSceneIndex.ToString());
        SceneManager.UnloadSceneAsync(thisSceneIndex);

        GameObject.FindWithTag("Player").GetComponent<Control>().SendMessage("RebindComponents");

        Destroy(this.gameObject);
    }
}