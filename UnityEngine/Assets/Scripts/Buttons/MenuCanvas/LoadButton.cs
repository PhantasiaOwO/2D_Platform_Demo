using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    private const string LOAD_AREA = "000";

    private int _sceneIndex;
    private Vector3 _start;

    public GameObject shade;
    public GameObject buttonParentCanvas;

    private void Start()
    {
        _sceneIndex = 0;
        // Load scene index from file
        try
        {
            var loadData = SaveSystem.LoadFile<PlayerStatus.PlayerStatusData>(PlayerStatus.PLAYER_STATUS_FILE_NAME);
            _sceneIndex = loadData.sav_sceneIndex;
            _start = loadData.sav_courseStart;
        }
        catch (Exception e)
        {
            GetComponent<Button>().interactable = false;
            GetComponent<Image>().color = new Color(100, 100, 100, 255);

#if UNITY_EDITOR
            Debug.LogError($"Load data try.{e}");
#endif
        }
    }

    public void ClickLoad()
    {
        Debug.Log("Click load button");

        Time.timeScale = 1;
        
        shade.SetActive(true);
        
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        var loadStatus1 = SceneManager.LoadSceneAsync(LOAD_AREA, LoadSceneMode.Additive);
        var loadStatus2 = SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Additive);

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
        var targetCourse = SceneManager.GetSceneByBuildIndex(_sceneIndex);
        SceneManager.MoveGameObjectToScene(player, targetCourse);
        SceneManager.MoveGameObjectToScene(camera, targetCourse);
        SceneManager.MoveGameObjectToScene(publicUI, targetCourse);
        SceneManager.MoveGameObjectToScene(shade, targetCourse);
        SceneManager.MoveGameObjectToScene(buttonParentCanvas, targetCourse);

        // Check point 
        GameObject.FindWithTag("Player").transform.position = _start;

        // Unload other scenes
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.UnloadSceneAsync(LOAD_AREA);

        GameObject.FindWithTag("Player").GetComponent<Control>().SendMessage("RebindComponents");
        
        // Player data added
        try
        {
            SaveSystem.LoadFile<PlayerStatus.PlayerStatusData>(PlayerStatus.PLAYER_STATUS_FILE_NAME);
            GameObject.FindWithTag("Player").GetComponent<PlayerStatus>().LoadPlayerStatusFile();
        }
        catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.LogError($"Load data try.{e}");
#endif
        }
        
        LoadEnd();
    }


    #region LoadEnd

    private void LoadEnd()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        buttonParentCanvas.SetActive(false);
        shade.SetActive(false);
        shade.GetComponent<LoadShade>().ShadeHide();
        
        Destroy(shade.gameObject);
        Destroy(buttonParentCanvas.gameObject);
    }

    #endregion
}