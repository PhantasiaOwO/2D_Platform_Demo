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

    #endregion

    private void Start()
    {
        remainTextBox.text = "Brick remain: " + brickNumber.ToString();

        // Get player game object
        // TODO Need test in scene change
        player = GameObject.Find("Player");
    }

    public void OnClick()
    {
        if (!GameObject.Find("Player").GetComponent<Control>().isOnGround)
        {
            // TODO Didn't standing on the ground
            Debug.Log("BrickInteraction: Didn't stand on the ground");
            return;
        }

        if (!GameObject.Find("Player").GetComponent<PlayerStatus>().canSpawn)
        {
            // TODO Print last brick unlock status information
            Debug.Log("BrickInteraction: Button banned");
            return;
        }

        if (brickNumber <= 0)
        {
            Debug.Log("BrickInteraction: Brick used up");
            // TODO Print used up information
            return;
        }

        // Spawn brick
        var playerPosition = player.transform.position;
        var newBrick = GameObject.Instantiate(brick);
        newBrick.transform.position =
            new Vector3(playerPosition.x + 2.5f, playerPosition.y + 1.5f, playerPosition.z);

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
    }
}