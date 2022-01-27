using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Scene = UnityEditor.SearchService.Scene;

public class LoadTrigger : MonoBehaviour
{
    public GameObject player;
    public new GameObject camera;

    private bool _isTriggered;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        camera = GameObject.FindWithTag("MainCamera");
        _isTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (_isTriggered) return;

        DontDestroyOnLoad(player);
        DontDestroyOnLoad(camera);

        StartCoroutine(LoadSceneAsync());
        _isTriggered = true;
    }

    IEnumerator LoadSceneAsync()
    {
        var thisSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var loadStatus = SceneManager.LoadSceneAsync(thisSceneIndex + 1, LoadSceneMode.Additive);

        while (!loadStatus.isDone)
        {
            yield return null;
        }

        // Move vital game object to new scene
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByBuildIndex(thisSceneIndex + 1));
        SceneManager.MoveGameObjectToScene(camera, SceneManager.GetSceneByBuildIndex(thisSceneIndex + 1));

        // Unload last scene will execute when enter unload trigger("UnloadTrigger.cs")
    }
}