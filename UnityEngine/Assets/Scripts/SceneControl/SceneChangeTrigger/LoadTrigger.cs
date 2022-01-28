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
    public GameObject menuEscape;

    private bool _isTriggered;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        camera = GameObject.FindWithTag("MainCamera");
        menuEscape = GameObject.Find("UIEscape");

        _isTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (_isTriggered) return;

        DontDestroyOnLoad(player);
        DontDestroyOnLoad(camera);
        DontDestroyOnLoad(menuEscape);

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
        var nextScene = SceneManager.GetSceneByBuildIndex(thisSceneIndex + 1);
        SceneManager.MoveGameObjectToScene(player, nextScene);
        SceneManager.MoveGameObjectToScene(camera, nextScene);
        SceneManager.MoveGameObjectToScene(menuEscape, nextScene);

        // Unload last scene will execute when enter unload trigger("UnloadTrigger.cs")
    }
}