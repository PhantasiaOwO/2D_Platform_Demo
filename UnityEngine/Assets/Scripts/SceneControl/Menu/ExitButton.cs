using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ClickSaveList()
    {
        // TODO Load Save list scene
    }

    public void ClickExit()
    {
        Debug.Log("Click Exit");
        Application.Quit();
    }
}
