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

    // Unity component
    public Text healthTextBox;

    // Public value
    public int sceneIndex;
    public int health;
    public Vector3 lastCheckPoint; // It will modified in "CollectionInteraction.cs"
    public bool courseClearCondition;
    public bool canSpawn;

    private void Start()
    {
        healthTextBox.text = "Health: " + health.ToString();

        // Start new course
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        lastCheckPoint = GetComponent<Transform>().position;
        SavePlayerStatusFile();

        canSpawn = true;
        courseClearCondition = false;
    }

    #region Health change

    public void HealthChange()
    {
        if (health <= 0)
        {
            // TODO Game end
        }

        healthTextBox.text = "Health: " + health.ToString();
    }

    #endregion

    [System.Serializable]
    public class PlayerStatusData
    {
        public int sav_sceneIndex;
        public int sav_health;
        public Vector3 sav_lastCheckPoint;
        public bool sav_courseClearCondition;
    }

    #region Save player status file

    public void SavePlayerStatusFile()
    {
        var playerStatusData = new PlayerStatusData
        {
            sav_sceneIndex = sceneIndex,
            sav_health = health,
            sav_lastCheckPoint = lastCheckPoint,
            sav_courseClearCondition = courseClearCondition,
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
        lastCheckPoint = loadData.sav_lastCheckPoint;
        courseClearCondition = loadData.sav_courseClearCondition;
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