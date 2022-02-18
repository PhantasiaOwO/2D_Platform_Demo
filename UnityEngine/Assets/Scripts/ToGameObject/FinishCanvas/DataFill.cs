using System;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class DataFill : MonoBehaviour
{
    public GameObject player;

    public Text collectionCntText;
    public Text deathCntText;
    public Text placeCntText;
    public Text restartCntText;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void RefreshData()
    {
        player = GameObject.FindWithTag("Player");
        var data = player.GetComponent<PlayerStatus>();

        collectionCntText.text = "任务完成个数：" + data.cntCourseClearCondition;
        deathCntText.text = "死亡次数：" + data.cntDeath;
        placeCntText.text = "放置次数：" + data.cntPlace;
        restartCntText.text = "重开次数：" + data.cntRestart;
    }
}