using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private const string LOAD_AREA = "000";
    private const string FIRST_COURSE = "001";

    // In order to turn off shade, Menu should be move to new scene
    public GameObject shade;
    public GameObject buttonParentCanvas;

    public void ClickNewGame()
    {
        Debug.Log("Click NewGame");

        shade.SetActive(true);
        // shade.GetComponent<LoadShade>().ShadeAppear();

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
        SceneManager.MoveGameObjectToScene(shade, targetCourse);
        SceneManager.MoveGameObjectToScene(buttonParentCanvas, targetCourse);

        // Game start
        GameObject.FindWithTag("Player").transform.position = new Vector3(-14.5f, -0.5f, 0);

        // Unload other scenes
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.UnloadSceneAsync(LOAD_AREA);

        GameObject.FindWithTag("Player").GetComponent<Control>().SendMessage("RebindComponents");

        LoadEnd();
    }

    #region LoadEnd

    private void LoadEnd()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        buttonParentCanvas.SetActive(false);
        shade.SetActive(false);
        shade.GetComponent<LoadShade>().ShadeHide();
        
        Destroy(shade.gameObject);
        Destroy(buttonParentCanvas.gameObject);
    }

    #endregion
}