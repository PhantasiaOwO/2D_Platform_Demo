using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class BrickPhysics : MonoBehaviour
{
    // private const string TEMP_NAME = "Temp";
    private const float Sensitive = 0.6f;

    #region Delete while placing

    private bool _placing;

    #endregion

    #region Unity component

    [SerializeField] private Rigidbody2D brickRigidbody;
    public Collider2D brickCollider;
    public Collider2D stuckDetectCollider;

    public GameObject sourceButton;
    public LayerMask ground;

    #endregion

    #region Brick mouse interaction

    private bool _canMove;

    #endregion

    #region Unity component

    private void Start()
    {
        brickRigidbody = GetComponent<Rigidbody2D>();
        brickCollider = GetComponent<PolygonCollider2D>();
        stuckDetectCollider = GetComponent<BoxCollider2D>();
        _canMove = true;
        _placing = true;
    }

    private void Update()
    {
        // After transfer delete
        if (this.transform.position.y < -50f)
        {
            Destroy(this.gameObject);
        }

        // Stuck delete
        if (stuckDetectCollider.IsTouchingLayers(ground))
        {
            Destroy(this.GetComponent<SpriteRenderer>());
            
            RelieveOtherGameObjects();
            
            var brickInteraction = sourceButton.GetComponent<BrickInteraction>();
            brickInteraction.remainTextBox.text = (++brickInteraction.brickNumber).ToString();
            
            var notice = GameObject.FindWithTag("Player").GetComponent<Notice>();
            notice.ShowNotice(notice.brickDestroyNotice);
            Destroy(this.gameObject);
            Cursor.visible = true;
            return;
        }

        if (!_placing) return;

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
            _placing = false;

            RelieveOtherGameObjects();
        }

        if (Input.GetButtonDown("Fire3"))
        {
            Destroy(this.gameObject); // BUG delete all bricks

            var brickInteraction = sourceButton.GetComponent<BrickInteraction>();
            brickInteraction.remainTextBox.text = (++brickInteraction.brickNumber).ToString();

            _canMove = false;
            _placing = false;
            
            Cursor.visible = true;
            
            RelieveOtherGameObjects();
        }
    }

    #endregion

    #region Player and Buttons relieve

    private void RelieveOtherGameObjects()
    {
        // Relieve "Player" static
        GameObject.Find("Player").SendMessage("ReceiveSpawnEnd");
        Debug.Log("BrickPhysics: Player can move because placement ended.");

        // Relieve "Button" available
        GameObject.Find("Player").GetComponent<PlayerStatus>().canSpawn = true;
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
        brickRigidbody.position += Vector2.right * Input.GetAxisRaw("Mouse X") * Sensitive;
        brickRigidbody.position += Vector2.up * Input.GetAxisRaw("Mouse Y") * Sensitive;
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