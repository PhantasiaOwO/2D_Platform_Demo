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
        var data = player.GetComponent<PlayerStatus>();
        
        collectionCntText.text = "任务完成个数：" + 12.ToString(); // TODO Add collection
        deathCntText.text = "死亡次数：" + data.cntDeath.ToString();
        placeCntText.text = "放置次数：" + data.cntPlace.ToString();
        restartCntText.text = "重开次数：" + data.cntRestart.ToString();
    }
}