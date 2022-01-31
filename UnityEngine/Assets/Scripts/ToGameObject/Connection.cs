using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connection : MonoBehaviour
{
    public GameObject dialog;
    public GameObject player;
    public new GameObject camera;
    public GameObject menuEscape;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        camera = GameObject.FindWithTag("MainCamera");
        menuEscape = GameObject.Find("UIEscape");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        // TODO Show dialog
        dialog.SetActive(true);
    }

    public void ClickLoad()
    {
        // Scene change
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(camera);
        DontDestroyOnLoad(menuEscape);

        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        var thisSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var loadStatus = SceneManager.LoadSceneAsync(thisSceneIndex + 1, LoadSceneMode.Additive);

        while (!loadStatus.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(thisSceneIndex + 1));

        // Move vital game object to new scene
        var nextScene = SceneManager.GetSceneByBuildIndex(thisSceneIndex + 1);
        SceneManager.MoveGameObjectToScene(player, nextScene);
        SceneManager.MoveGameObjectToScene(camera, nextScene);
        SceneManager.MoveGameObjectToScene(menuEscape, nextScene);

        // Unload last scene
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(thisSceneIndex));
    }
}