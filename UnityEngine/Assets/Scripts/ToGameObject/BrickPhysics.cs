using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class BrickPhysics : MonoBehaviour
{
    #region Unity component

    [SerializeField] private Rigidbody2D brickRigidbody;
    [SerializeField] private Collider2D brickCollider;
    public GameObject sourceButton;

    #endregion

    #region Status boolean

    private bool _canMove;

    #endregion

    #region Unity component

    private void Start()
    {
        brickRigidbody = GetComponent<Rigidbody2D>();
        brickCollider = GetComponent<Collider2D>();
        _canMove = true;
    }

    private void Update()
    {
        // Set other object when lock brick
        if (Input.GetButtonDown("Fire2"))
        {
            // Brick status set
            brickRigidbody.gravityScale = 1;
            brickRigidbody.mass = 1000000;
            brickCollider.isTrigger = false;

            var color = brickRigidbody.GetComponent<SpriteRenderer>().color;
            brickRigidbody.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);

            _canMove = false;

            // Relieve "Player" static
            GameObject.Find("Player").SendMessage("ReceiveSpawnEnd");
            Debug.Log("BrickPhysics: Player can move because placement ended.");

            // Relieve "Button" available
            GameObject.Find("Player").GetComponent<PlayerStatus>().canSpawn = true;
        }
        
        // Transfer detect
        if (this.transform.position.y < -50f)
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    #region DeadZone

    // Only trigger DeadZone can destroy brick
    // It means that only fallen can trigger this method
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("DeadZone")) return;

        Debug.Log("Brick fall out");
        
        Destroy(brickCollider.gameObject);

        // Print deleted information
        // OLD: GameObject.FindWithTag("Player").GetComponent<NoticeDialog>().ShowBrickDestroyNotice();
        var notice = GameObject.FindWithTag("Player").GetComponent<Notice>();
        notice.ShowNotice(notice.brickDestroyNotice);
        
        // Modify the value of "BrickInteraction"
        var brickNumberTemp = ++sourceButton.GetComponent<BrickInteraction>().brickNumber;
        sourceButton.GetComponent<BrickInteraction>().remainTextBox.text = brickNumberTemp.ToString();

        // Set other object when placement ended because the brick destroyed
        // Relieve "Player" static
        GameObject.Find("Player").SendMessage("ReceiveSpawnEnd");
        Debug.Log("BrickPhysics: Player can move because placement ended.");

        // Relieve "Button" available
        GameObject.Find("Player").GetComponent<PlayerStatus>().canSpawn = true;

        // Cursor visible when brick fall out
        Cursor.visible = true;
    }

    #endregion

    #region Mouse control

    private void OnMouseDown()
    {
        if (!_canMove) return;

        Cursor.visible = false;
        brickRigidbody.gravityScale = 0;
        brickRigidbody.mass = 1;
        brickCollider.isTrigger = false;
    }

    private void OnMouseDrag()
    {
        if (!_canMove) return;

        // Position follow cursor
        brickRigidbody.position += Vector2.right * Input.GetAxis("Mouse X");
        brickRigidbody.position += Vector2.up * Input.GetAxis("Mouse Y");
    }

    private void OnMouseUp()
    {
        if (!_canMove) return;

        Cursor.visible = true;
        brickRigidbody.gravityScale = 1;
        brickRigidbody.mass = 1000000;
    }

    #endregion
}