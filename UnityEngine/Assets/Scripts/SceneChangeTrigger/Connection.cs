using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connection : MonoBehaviour
{
    const string TEMP_NAME = "Connection(Old)";
    
    public GameObject dialog;
    public GameObject player;
    public new GameObject camera;
    public GameObject publicUI;
    public Vector3 destination;

    private bool _trigger;

    [HideInInspector] public Animator animator;

    private bool _isSubscribed;
    
    private static readonly int Appear = Animator.StringToHash("Appear");

    private void Start()
    {
        dialog.SetActive(true);

        _isSubscribed = false;
        
        player = GameObject.FindWithTag("Player");
        camera = GameObject.FindWithTag("MainCamera");
        publicUI = GameObject.Find("UIPublic");

        animator = dialog.GetComponent<Animator>();
        animator.SetBool(Appear,false);
        Debug.Log("Initialize dialog " + gameObject.ToString());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (!player.GetComponent<PlayerStatus>().courseClearCondition)
        {
            var notice = player.GetComponent<Notice>();
            notice.ShowNotice(notice.noCourseClearConditionNotice);
            return;
        }

        animator.SetBool(Appear, true);
        Debug.Log(gameObject.ToString() + "Dialog appear");

        _trigger = true;

        if (_isSubscribed) return;
        // Subscribe event after trigger enter
        SceneManager.sceneUnloaded += OnSceneUnload;
        _isSubscribed = true;

        this.gameObject.name = TEMP_NAME;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_trigger) return;
        if (!other.CompareTag("Player")) return;
        if (!player.GetComponent<PlayerStatus>().courseClearCondition)
        {
            var notice = player.GetComponent<Notice>();
            notice.ShowNotice(notice.noCourseClearConditionNotice);
            return;
        }
        if (!Input.GetKeyDown(KeyCode.E)) return;

        _trigger = false;

        // Scene change
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(camera);
        DontDestroyOnLoad(publicUI);

        StartCoroutine(LoadSceneAsync());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        animator.SetBool(Appear, false);
        Debug.Log(gameObject.ToString() + "Dialog disappear");
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
        SceneManager.MoveGameObjectToScene(publicUI, nextScene);

        // Teleport player
        player.transform.position = destination;

        // Unload last scene
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(thisSceneIndex));
    }
    
    private void OnSceneUnload(UnityEngine.SceneManagement.Scene arg0)
    {
        GameObject.FindWithTag("Player").GetComponent<Control>().SendMessage("RebindComponents");
        GameObject.FindWithTag("Player").GetComponent<PlayerStatus>().courseClearCondition = false;
        Destroy(GameObject.Find(TEMP_NAME));
    }
}