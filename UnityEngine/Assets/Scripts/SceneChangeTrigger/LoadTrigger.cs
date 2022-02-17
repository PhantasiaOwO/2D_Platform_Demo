using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/**
 * 卸载场景前移动必要的组件（玩家、摄像机、Escape菜单、加载触发器）
 * 通过订阅事件检测场景卸载（触发器触发时），避免因为多场景导致的GameObject绑定的问题
 * 完成所有工作后Destroy这个GameObject
 */
public class LoadTrigger : MonoBehaviour
{
    const string TEMP_NAME = "LoadTrigger(Old)";
    
    public GameObject player;
    public new GameObject camera;
    public GameObject publicUI;

    private bool _isTriggered;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        camera = GameObject.FindWithTag("MainCamera");
        publicUI = GameObject.Find("UIPublic");

        _isTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (!GameObject.FindWithTag("Player").GetComponent<PlayerStatus>().courseClearCondition)
        {
            // Show no course clear condition notice
            // OLD: GameObject.FindWithTag("Player").GetComponent<NoticeDialog>().ShowNoCourseClearCondition();
            var notice = GameObject.FindWithTag("Player").GetComponent<Notice>();
            notice.ShowNotice(notice.noCourseClearConditionNotice);
            return;
        }

        if (_isTriggered) return;

        // Subscribe event after trigger enter
        SceneManager.sceneUnloaded += OnSceneUnload;

        this.gameObject.name = TEMP_NAME;
        
        StartCoroutine(LoadSceneAsync());
        _isTriggered = true;

        // If destroy, coroutine will be closed, which can't ensure the codes are run
        // Destroy(this.gameObject);
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
        SceneManager.MoveGameObjectToScene(publicUI, nextScene);
        SceneManager.MoveGameObjectToScene(this.gameObject, nextScene);
        Debug.Log("Move Game object");

        // Move all spawned bricks
        var bricks = GameObject.FindGameObjectsWithTag("Spawn");
        foreach (var brick in bricks)
        {
            SceneManager.MoveGameObjectToScene(brick,nextScene);
        }
        
        SceneManager.UnloadSceneAsync(thisSceneIndex);
    }

    private void OnSceneUnload(UnityEngine.SceneManagement.Scene arg0)
    {
        GameObject.FindWithTag("Player").GetComponent<Control>().SendMessage("RebindComponents");
        GameObject.FindWithTag("Player").GetComponent<PlayerStatus>().courseClearCondition = false;
        // TODO Turn off game object condition
        Destroy(GameObject.Find(TEMP_NAME));
    }
}