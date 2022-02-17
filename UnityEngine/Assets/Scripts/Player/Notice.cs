using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notice : MonoBehaviour
{
    public GameObject deathCanvas;

    #region General notice

    public GameObject brickDestroyNotice;
    public GameObject noCourseClearConditionNotice;
    public GameObject notOnGroundNotice;
    public GameObject notLockBrickNotice;
    public GameObject notEnoughBrickNotice;
    
    private static readonly int Appear = Animator.StringToHash("Appear");

    #endregion

    private void Start()
    {
        HideAllNotice();
    }

    private void HideAllNotice()
    {
        deathCanvas.SetActive(false);
        brickDestroyNotice.SetActive(false);
        noCourseClearConditionNotice.SetActive(false);
        notOnGroundNotice.SetActive(false);
        notLockBrickNotice.SetActive(false);
        notEnoughBrickNotice.SetActive(false);

        Debug.Log("Notice: Hide all notice.");
    }

    #region ShowNotice public method

    public void ShowNotice(GameObject fieldInNoticeScript)
    {
        HideAllNotice();

        fieldInNoticeScript.SetActive(true);
        // TODO Animator

        StartCoroutine(OffGameObject(fieldInNoticeScript));
    }

    private static IEnumerator OffGameObject(GameObject gameObj)
    {
        yield return new WaitForSecondsRealtime(5f);

        // TODO Animator
        gameObj.SetActive(false);
    }

    #endregion

    #region Death

    public void ShowDeathNotice()
    {
        deathCanvas.SetActive(true);
    }

    public void HideDeathNotice()
    {
        // TODO Animator
        var animator = deathCanvas.GetComponent<Animator>();
        animator.SetBool(Appear, false);

        deathCanvas.SetActive(false);
    }

    #endregion

    #region Obsoleted methods

    #region Brick destory

    public void ShowBrickDestroyNotice()
    {
        brickDestroyNotice.SetActive(true);
        // TODO Animator

        StartCoroutine(OffGameObject(brickDestroyNotice));
    }

    #endregion

    #region No course clear condition

    public void ShowNoCourseClearCondition()
    {
        noCourseClearConditionNotice.SetActive(true);
        // TODO Animator

        StartCoroutine(OffGameObject(noCourseClearConditionNotice));
    }

    #endregion

    #region Not on ground

    public void ShowNoStandNotice()
    {
        notOnGroundNotice.SetActive(true);
        // TODO Animator

        StartCoroutine(OffGameObject(notOnGroundNotice));
    }

    #endregion

    #endregion
}