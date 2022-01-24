using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    private int sceneIndex;

    private void Start()
    {
        sceneIndex = 0;
        PlayerStatus.PlayerStatusData loadData;
        // Load scene index from file
        try
        {
            loadData = SaveSystem.LoadFile<PlayerStatus.PlayerStatusData>(PlayerStatus.PLAYER_STATUS_FILE_NAME);
            sceneIndex = loadData.sav_sceneIndex;
        }
        catch (Exception e)
        {
            GetComponent<Button>().interactable = false;

#if UNITY_EDITOR
            Debug.LogError($"Load data try.{e}");
#endif
        }
    }

    public void ClickLoad()
    {
        // Go to target scene
        SceneManager.LoadScene(sceneIndex);
        Debug.Log("Click load button");
    }
}