using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    // Const
    private const string PLAYER_STATUS_KEY = "PlayerStatus";
    public const string PLAYER_STATUS_FILE_NAME = "PlayerStatus.dat";

    // Public value
    public int sceneIndex;
    public int health;
    public Vector3 courseStart; // It will modified in "CollectionInteraction.cs"
    public bool courseClearCondition;
    public bool canSpawn;

    // Data count value
    [HideInInspector] public int cntDeath;
    [HideInInspector] public int cntPlace;
    [HideInInspector] public int cntRestart;

    private void Start()
    {
        // healthTextBox.text = "Health: " + health.ToString();

        // // Start new course
        // sceneIndex = SceneManager.GetActiveScene().buildIndex;
        // courseStart = GetComponent<Transform>().position;
        // SavePlayerStatusFile();

        canSpawn = true;
        courseClearCondition = false;
    }

    #region Health change

    public void HealthChange()
    {
        if (health <= 0)
        {
            // TODO Game end notice
        }

        // healthTextBox.text = "Health: " + health.ToString();
    }

    #endregion

    [System.Serializable]
    public class PlayerStatusData
    {
        public int sav_sceneIndex;
        public int sav_health;
        public Vector3 sav_courseStart;
        public int sav_cntDeath;
        public int sav_cntPlace;
        public int sav_cntRestart;
    }

    #region Save player status file

    public void SavePlayerStatusFile()
    {
        var playerStatusData = new PlayerStatusData
        {
            sav_sceneIndex = sceneIndex,
            sav_health = health,
            sav_courseStart = courseStart,
            sav_cntDeath = cntDeath,
            sav_cntPlace = cntPlace,
            sav_cntRestart = cntRestart,
        };

        SaveSystem.SaveFile(PLAYER_STATUS_FILE_NAME, playerStatusData);
    }

    #endregion

    #region Load player status file

    public void LoadPlayerStatusFile()
    {
        var loadData = SaveSystem.LoadFile<PlayerStatusData>(PLAYER_STATUS_FILE_NAME);

        sceneIndex = loadData.sav_sceneIndex;
        health = loadData.sav_health;
        courseStart = loadData.sav_courseStart;
        cntDeath = loadData.sav_cntDeath;
        cntPlace = loadData.sav_cntPlace;
        cntRestart = loadData.sav_cntRestart;
    }

    #endregion

    #region Delete player status file

#if UNITY_EDITOR
    [MenuItem("Developer/Delete Player Status File")]
#endif
    public static void DeletePlayerStatusFile()
    {
        SaveSystem.DeleteFile(PLAYER_STATUS_FILE_NAME);
    }

    #endregion
}