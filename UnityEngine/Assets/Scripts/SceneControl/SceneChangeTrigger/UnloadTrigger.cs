using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Unload scene
        var thisSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("This scene is " + thisSceneIndex.ToString());
        SceneManager.UnloadSceneAsync(thisSceneIndex);

        // Rebind camera confiner
        GameObject.FindWithTag("MainCamera").GetComponentInChildren<CinemachineConfiner>().m_BoundingShape2D =
            GameObject.FindWithTag("BackGround").GetComponent<Collider2D>();

        // Rebind player scripts of UI canvas
        GameObject.FindWithTag("Player").GetComponent<Interaction>().conditionUI = GameObject.Find("UIGame/Condition");
        // GameObject.FindWithTag("Player").GetComponent<Control>().gameObjectEsc =
        //     GameObject.Find("UIEscape").transform.Find("UIESC").gameObject;
    }
}