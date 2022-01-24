using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Control : MonoBehaviour
{
    #region Unity component

    public Rigidbody2D playerRigidbody;
    public Collider2D bodyCollider;
    public Collider2D feetCollider;
    public LayerMask ground;
    public GameObject gameObjectEsc;

    #endregion

    #region Public value

    public float normalSpeed;
    public float jumpSpeed;
    public float jumpForce;

    #endregion

    #region Boolean to transmit status

    private bool _isJump;
    private bool _isBuilding;
    private bool _isHurt;
    private bool _isPause;

    #endregion

    #region Unity method

    void Start()
    {
        // Get component
        playerRigidbody = GetComponent<Rigidbody2D>();

        // Initialize boolean
        _isJump = false;
        _isBuilding = false;
        _isHurt = false;
        _isPause = false;

        Time.timeScale = 1;
    }

    void Update()
    {
        if (!_isPause && Input.GetKeyDown(KeyCode.Escape))
        {
            _isPause = true;
            gameObjectEsc.SetActive(true);
            Time.timeScale = 0;
        }
        else if (_isPause && Input.GetKeyDown(KeyCode.Escape))
        {
            _isPause = false;
            gameObjectEsc.SetActive(false);
            Time.timeScale = 1;
        }

        JudgeJump();
        BeHurt();
    }

    private void FixedUpdate()
    {
        // When building, lock the velocity and pass "Move" and "Jump"
        if (_isBuilding)
        {
            playerRigidbody.velocity = new Vector2(0f, 0f);
            return;
        }

        Move();
        ExecuteJump();
    }

    #endregion

    #region Fallout action

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("DeadZone")) return;
        _isHurt = true;
        Debug.Log("trigger enter");
    }

    private void BeHurt()
    {
        if (!_isHurt) return;

        StartCoroutine(RunFallOut(GetComponent<PlayerStatus>().lastCheckPoint));
        _isHurt = false;
    }

    private IEnumerator RunFallOut(Vector3 checkPoint)
    {
        yield return new WaitForSeconds(3f);

        // Decrease health
        GetComponent<PlayerStatus>().health--;
        GetComponent<PlayerStatus>().HealthChange();
        playerRigidbody.transform.position = checkPoint;
        yield break;
    }

    #endregion

    #region Move

    private void Move()
    {
        if (feetCollider.IsTouchingLayers(ground))
        {
            // On ground speed
            var horizontal = Input.GetAxis("Horizontal");
            playerRigidbody.velocity =
                new Vector2(normalSpeed * horizontal * Time.deltaTime, playerRigidbody.velocity.y);
            // TODO Animator player face direction
        }
        else
        {
            // On air speed
            var horizontal = Input.GetAxis("Horizontal");
            playerRigidbody.velocity =
                new Vector2(jumpSpeed * horizontal * Time.deltaTime, playerRigidbody.velocity.y);
        }
    }

    #endregion

    #region Jump

    private void JudgeJump()
    {
        if (!feetCollider.IsTouchingLayers(ground)) return;
        if (!Input.GetButtonDown("Jump")) return;

        _isJump = true;
    }

    private void ExecuteJump()
    {
        if (!_isJump) return;
        _isJump = false;

        Debug.Log("Player.Movement: Execute jump");
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce * Time.deltaTime);
        // TODO Animator jump
    }

    #endregion

    #region Message spawn

    void ReceiveSpawnStart() // "BrickInteraction" will use this method
    {
        Debug.Log("Player.Movement: Received spawn start");
        _isBuilding = true;
        playerRigidbody.mass = 1000000;
    }

    void ReceiveSpawnEnd() // "BrickPhysics" will use this method
    {
        Debug.Log("Player.Movement: Received spawn end");
        _isBuilding = false;
        playerRigidbody.mass = 1;
    }

    #endregion
}