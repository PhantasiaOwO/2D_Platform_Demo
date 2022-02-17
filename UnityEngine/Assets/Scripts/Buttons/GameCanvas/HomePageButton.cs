using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HomePageButton : MonoBehaviour
{
    public GameObject twiceConfirmPanel;

    private void Start()
    {
        twiceConfirmPanel.SetActive(false);
    }

    public void ClickHomePage()
    {
        Debug.Log("Escape: Click home");

        twiceConfirmPanel.SetActive(true);
    }
}