using System;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public GameObject finishCanvas;

    private void Start()
    {
        finishCanvas.SetActive(false);
    }

    public void ShowFinishCanvas()
    {
        finishCanvas.SetActive(true);
        // Animator will auto run
    }
}