using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private const string LOAD_AREA = "000";
    private const string FIRST_COURSE = "001";

    public void ClickNewGame()
    {
        Debug.Log("Click NewGame");

        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        var loadStatus1 = SceneManager.LoadSceneAsync(LOAD_AREA, LoadSceneMode.Additive);
        var loadStatus2 = SceneManager.LoadSceneAsync(FIRST_COURSE, LoadSceneMode.Additive);

        while (!(loadStatus1.isDone && loadStatus2.isDone))
        {
            yield return null;
        }

        // Get game object
        var player = GameObject.FindWithTag("Player");
        var camera = GameObject.FindWithTag("MainCamera");
        var publicUI = GameObject.Find("UIPublic");

        DontDestroyOnLoad(player);
        DontDestroyOnLoad(camera);
        DontDestroyOnLoad(publicUI);

        // Move game object
        var targetCourse = SceneManager.GetSceneByName(FIRST_COURSE);
        SceneManager.MoveGameObjectToScene(player, targetCourse);
        SceneManager.MoveGameObjectToScene(camera, targetCourse);
        SceneManager.MoveGameObjectToScene(publicUI, targetCourse);

        // Game start
        GameObject.FindWithTag("Player").transform.position = new Vector3(-14.5f, -0.5f, 0);

        // Unload other scenes
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.UnloadSceneAsync(LOAD_AREA);

        GameObject.FindWithTag("Player").GetComponent<Control>().SendMessage("RebindComponents");
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