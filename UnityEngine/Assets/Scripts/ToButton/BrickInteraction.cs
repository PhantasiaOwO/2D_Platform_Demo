using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BrickInteraction : MonoBehaviour
{
    #region Unity component

    public GameObject player;
    [FormerlySerializedAs("brickT")] public GameObject brick;
    public Text remainTextBox;

    # endregion

    #region Public value

    public int brickNumber;
    [HideInInspector] public int brickInit;

    #endregion

    private void Start()
    {
        brickInit = brickNumber;
        remainTextBox.text = brickNumber.ToString();

        // Get player game object
        player = GameObject.Find("Player");
    }

    public void OnClick()
    {
        var notice = GameObject.FindWithTag("Player").GetComponent<NoticeDialog>();
        if (!GameObject.Find("Player").GetComponent<Control>().isOnGround)
        {
            // Notice didn't standing on the ground
            notice.ShowNotice(notice.notOnGroundNotice);

            Debug.Log("BrickInteraction: Didn't stand on the ground");
            return;
        }

        if (!GameObject.Find("Player").GetComponent<PlayerStatus>().canSpawn)
        {
            // Notice Print last brick unlock status information
            notice.ShowNotice(notice.notLockBrickNotice);

            Debug.Log("BrickInteraction: Button banned");
            return;
        }

        if (brickNumber <= 0)
        {
            // TODO Print used up information
            notice.ShowNotice(notice.notEnoughBrickNotice);

            Debug.Log("BrickInteraction: Brick used up");
            return;
        }

        // Spawn brick
        var playerPosition = player.transform.position;
        var newBrick = GameObject.Instantiate(brick);

        newBrick.transform.position = new Vector3(
            playerPosition.x + player.transform.localScale.x / 0.02f * 1.5f,
            playerPosition.y + 1.5f,
            playerPosition.z);
        newBrick.tag = "Spawn";

        var color = newBrick.GetComponent<SpriteRenderer>().color;
        newBrick.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.5f);

        newBrick.name = "(Spawn)" + brick.name.ToString();
        newBrick.GetComponent<Rigidbody2D>().gravityScale = 0;
        newBrick.GetComponent<Collider2D>().isTrigger = false;
        newBrick.SetActive(true);

        // Require "Player" static
        GameObject.Find("Player").SendMessage("ReceiveSpawnStart");
        Debug.Log("BrickInteraction: Brick \"" + brick.ToString() + "\" spawned");

        // Require ban button press
        GameObject.Find("Player").GetComponent<PlayerStatus>().canSpawn = false;

        // Count and print brick number
        remainTextBox.text = (--brickNumber).ToString();

        // Count data
        GameObject.FindWithTag("Player").GetComponent<PlayerStatus>().cntPlace++;
    }
}