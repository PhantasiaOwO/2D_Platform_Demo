using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectionInteraction : MonoBehaviour
{
    // Unity component
    public Text conditionTextBox;

    private void Start()
    {
        #region Condition Initiaization

        GetComponent<PlayerStatus>().courseClearCondition = false;
        var color = Color.green;
        conditionTextBox.color = new Color(color.r, color.g, color.b, 0);

        #endregion
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Notice: Body collider and feet collider will both active this method
        #region Course Clear Condition

        if (col.CompareTag("Course Clear Condition"))
        {
            Destroy(col.gameObject);
            GetComponent<PlayerStatus>().courseClearCondition = true;
            var color = conditionTextBox.color;
            conditionTextBox.color = new Color(color.r, color.g, color.b, 1);
        }

        #endregion

        #region CheckPoint

        if (col.CompareTag("CheckPoint"))
        {
            var PositionCP = col.transform.position;
            // Send information to "PlayerStatus"
            GetComponent<PlayerStatus>().lastCheckPoint = PositionCP;
            GetComponent<PlayerStatus>().sceneIndex = SceneManager.GetActiveScene().buildIndex;
            GetComponent<PlayerStatus>().SavePlayerStatusFile(); // Save data when touching check point
            Debug.Log("CheckPoint change to " + PositionCP.ToString());
            Destroy(col.gameObject);
        }

        #endregion
    }
}