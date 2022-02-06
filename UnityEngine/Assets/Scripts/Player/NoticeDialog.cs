using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeDialog : MonoBehaviour
{
    public GameObject deathNotice;
    public GameObject brickDestroyNotice;
    public GameObject noCourseClearConditionNotice;

    #region Spawn

    public GameObject notOnGroundNotice;
    public GameObject notLockBrickNotice;
    public GameObject notEnoughBrickNotice;

    #endregion

    private void Start()
    {
        deathNotice.SetActive(false);
        brickDestroyNotice.SetActive(false);
        noCourseClearConditionNotice.SetActive(false);
        notOnGroundNotice.SetActive(false);
        notLockBrickNotice.SetActive(false);
        notEnoughBrickNotice.SetActive(false);
    }

    public void ShowNotice(GameObject gameObj)
    {
        gameObj.SetActive(true);
        // TODO Animator

        StartCoroutine(OffGameObject(gameObj));
    }

    private static IEnumerator OffGameObject(GameObject gameObj)
    {
        yield return new WaitForSecondsRealtime(1f);

        // TODO Animator
        gameObj.SetActive(false);
    }

    #region Death

    public void ShowDeathNotice()
    {
        // TODO Animator
        deathNotice.SetActive(true);
    }

    public void HideDeathNotice()
    {
        deathNotice.SetActive(false);
        // TODO Animator
    }

    #endregion

    #region Old methods

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