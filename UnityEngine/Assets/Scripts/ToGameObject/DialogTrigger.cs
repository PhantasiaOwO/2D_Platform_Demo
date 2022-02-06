using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DialogTrigger : MonoBehaviour
{
    public GameObject dialog;
    [HideInInspector] public Animator animator;

    private static readonly int Appear = Animator.StringToHash("Appear");

    private void Start()
    {
        dialog.SetActive(true);

        animator = dialog.GetComponent<Animator>();
        Debug.Log("Initialize dialog " + gameObject.ToString());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        animator.SetBool(Appear, true);
        Debug.Log(gameObject.ToString() + "Dialog appear");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        animator.SetBool(Appear, false);
        Debug.Log(gameObject.ToString() + "Dialog disappear");
    }
}