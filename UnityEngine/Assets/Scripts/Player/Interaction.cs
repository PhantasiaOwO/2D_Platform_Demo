using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    // Unity component
    public GameObject conditionUI;

    private void Start()
    {
        conditionUI = GameObject.Find("UIGame/Condition");

        GetComponent<PlayerStatus>().courseClearCondition = false;
        conditionUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Notice: Body collider and feet collider will both active this method

        #region Course Clear Condition

        if (col.CompareTag("Course Clear Condition"))
        {
            Destroy(col.gameObject);
            GetComponent<PlayerStatus>().courseClearCondition = true;
            conditionUI.SetActive(true);
        }

        #endregion

        #region CheckPoint

        if (col.CompareTag("CheckPoint"))
        {
            var checkPointPosition = col.transform.position;
            // Send information to "PlayerStatus"
            GetComponent<PlayerStatus>().courseStart = checkPointPosition;
            GetComponent<PlayerStatus>().sceneIndex = SceneManager.GetActiveScene().buildIndex;
            GetComponent<PlayerStatus>().SavePlayerStatusFile(); // Save data when touching check point
            Debug.Log("CheckPoint change to " + checkPointPosition.ToString());
            Destroy(col.gameObject);
        }

        #endregion
    }
}