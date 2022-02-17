using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    #region Unity component

    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Animator playerAnimator;
    public Collider2D bodyCollider;
    public Collider2D groundDetectCollider;

    public LayerMask ground;
    // Change UI format
    // public GameObject gameObjectEsc;

    #endregion

    #region Public value

    public float normalSpeed;
    public float jumpSpeed;
    public float jumpForce;
    public bool isOnGround;

    #endregion

    #region Boolean to transmit status

    [HideInInspector] public bool canMove;

    private bool _isJumping;
    private bool _isBuilding;
    private bool _hurtTrigger;
    private bool _resetTrigger;
    
    // Change UI format
    // private bool _isPaused;

    #endregion

    #region Animator index

    private static readonly int BlnAnimIdle = Animator.StringToHash("idle");
    private static readonly int FltAnimRun = Animator.StringToHash("running");
    private static readonly int BlnAnimRun = Animator.StringToHash("runningTrigger");
    private static readonly int BlnAnimJump = Animator.StringToHash("jumping");
    private static readonly int BlnAnimFall = Animator.StringToHash("falling");
    private static readonly int BlnAnimSummon = Animator.StringToHash("summon");

    #endregion

    #region Unity method

    void Start()
    {
        // Get component
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        // Initialize boolean
        _isJumping = false;
        _isBuilding = false;
        _hurtTrigger = false;
        _resetTrigger = false;
        // Change UI format
        // _isPaused = false;

        canMove = true;

        Time.timeScale = 1;

        playerAnimator.SetBool(BlnAnimIdle, true);
        playerAnimator.SetBool(BlnAnimJump, false);
        playerAnimator.SetBool(BlnAnimFall, false);
        playerAnimator.SetBool(BlnAnimSummon, false);
        playerAnimator.SetBool(BlnAnimRun, false);
        playerAnimator.SetFloat(FltAnimRun, 0f);
    }

    void Update()
    {
        // Change UI format
        // if (!_isPaused && Input.GetKeyDown(KeyCode.Escape))
        // {
        //     _isPaused = true;
        //     gameObjectEsc.SetActive(true);
        //     Time.timeScale = 0;
        // }
        // else if (_isPaused && Input.GetKeyDown(KeyCode.Escape))
        // {
        //     _isPaused = false;
        //     gameObjectEsc.SetActive(false);
        //     Time.timeScale = 1;
        // }

        JudgeJump();
        BeHurt();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            playerRigidbody.velocity = new Vector2(0, 0);
            return;
        }

        // When building, lock the velocity and pass "Move" and "Jump"
        // Same as canMove, but character should fall down
        if (_isBuilding)
        {
            var velocity = playerRigidbody.velocity;
            playerRigidbody.velocity = new Vector2(0f, velocity.y);
            return;
        }

        Move();
        ExecuteJump();
    }

    #endregion

    #region Dead action

    private int _secondCnt;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("DeadZone")) return;
        _hurtTrigger = true;
        Debug.Log("trigger enter");

        GetComponent<PlayerStatus>().cntDeath++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("DeadZone")) return;
        _hurtTrigger = true;
        Debug.Log("trigger enter");

        GetComponent<PlayerStatus>().cntDeath++;
    }

    private void BeHurt()
    {
        if (!_hurtTrigger) return;

        GetComponent<Notice>().ShowDeathNotice();

        // Trigger
        _hurtTrigger = false;
        _resetTrigger = true;
        canMove = false;

        // Count down 3 seconds
        _secondCnt = 3;

        // Animation
        playerAnimator.SetBool(BlnAnimIdle, true);
        playerAnimator.SetBool(BlnAnimJump, false);
        playerAnimator.SetBool(BlnAnimFall, false);
        playerAnimator.SetBool(BlnAnimSummon, false);
        playerAnimator.SetBool(BlnAnimRun, false);
        playerAnimator.SetFloat(FltAnimRun, 0f);

        StartCoroutine(ResetScene(GetComponent<PlayerStatus>().courseStart));
    }

    private IEnumerator ResetScene(Vector3 checkPoint)
    {
        var textBox = GameObject.FindWithTag("DeathSecondTextBox").GetComponent<Text>();

        while (_secondCnt > 0)
        {
            textBox.text = "重生剩余" + _secondCnt--.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }

        // Go to check point
        playerRigidbody.transform.position = GetComponent<PlayerStatus>().courseStart;

        // Remove all spawn bricks
        var bricks = GameObject.FindGameObjectsWithTag("Spawn");
        foreach (var brick in bricks)
        {
            Destroy(brick);
        }

        // Reset button
        var buttons = GameObject.FindGameObjectsWithTag("BrickButton");
        foreach (var button in buttons)
        {
            button.GetComponent<BrickInteraction>().brickNumber = button.GetComponent<BrickInteraction>().brickInit;
            button.GetComponent<BrickInteraction>().remainTextBox.text =
                button.GetComponent<BrickInteraction>().brickNumber.ToString();
            Debug.Log("Reset button");
        }

        GetComponent<Notice>().HideDeathNotice();
        _resetTrigger = false;
        canMove = true;
    }

    #endregion

    #region Move

    private void Move()
    {
        // Face direction
        var rawHorizontal = Input.GetAxisRaw("Horizontal");
        if (rawHorizontal != 0)
        {
            // Anim run trigger
            playerAnimator.SetBool(BlnAnimRun, true);

            var scale = playerRigidbody.transform.localScale;
            // If face and speed both direction,ignore these code
            if (scale.x * rawHorizontal < 0)
            {
                Debug.Log("Control: Face direction changed");
                playerRigidbody.transform.localScale = new Vector3(rawHorizontal * 0.02f, 0.02f, 1);
                if (rawHorizontal > 0)
                {
                    var position = playerRigidbody.transform.position;
                    playerRigidbody.transform.position = new Vector2(position.x + 0.65f, position.y);
                }
                else
                {
                    var position = playerRigidbody.transform.position;
                    playerRigidbody.transform.position = new Vector2(position.x - 0.65f, position.y);
                }
            }
        }
        else
        {
            playerAnimator.SetBool(BlnAnimRun, false);
        }

        // Movement
        if (groundDetectCollider.IsTouchingLayers(ground))
        {
            isOnGround = true;

            // On ground speed
            var horizontal = Input.GetAxis("Horizontal");
            var speed = playerRigidbody.velocity;
            playerRigidbody.velocity = new Vector2(normalSpeed * horizontal * Time.deltaTime, speed.y);

            // Animator run
            playerAnimator.SetFloat(FltAnimRun, Math.Abs(speed.x));
            playerAnimator.SetBool(BlnAnimFall, false);
        }
        else
        {
            isOnGround = false;

            // On air speed
            var horizontal = Input.GetAxis("Horizontal");
            var speed = playerRigidbody.velocity;
            playerRigidbody.velocity = new Vector2(jumpSpeed * horizontal * Time.deltaTime, speed.y);

            // Animator jump and fall
            if (speed.y < 0f)
            {
                playerAnimator.SetBool(BlnAnimJump, false);
                playerAnimator.SetBool(BlnAnimFall, true);
            }
        }
    }

    #endregion

    #region Jump

    private void JudgeJump()
    {
        if (!groundDetectCollider.IsTouchingLayers(ground)) return;
        if (!Input.GetButtonDown("Jump")) return;

        _isJumping = true;
    }

    private void ExecuteJump()
    {
        if (!_isJumping) return;
        _isJumping = false;

        Debug.Log("Player.Movement: Execute jump");
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce * Time.deltaTime);

        // Animation jump
        playerAnimator.SetBool(BlnAnimJump, true);
    }

    #endregion

    #region Message spawn

    void ReceiveSpawnStart() // "BrickInteraction" will use this method
    {
        Debug.Log("Player.Movement: Received spawn start");
        _isBuilding = true;
        playerRigidbody.mass = 1000000;
        playerAnimator.SetBool(BlnAnimIdle, true);
        playerAnimator.SetBool(BlnAnimFall, false);
        playerAnimator.SetBool(BlnAnimJump, false);
        playerAnimator.SetBool(BlnAnimRun, false);
        playerAnimator.SetBool(BlnAnimSummon, true);
    }

    void ReceiveSpawnEnd() // "BrickPhysics" will use this method
    {
        Debug.Log("Player.Movement: Received spawn end");
        _isBuilding = false;
        playerRigidbody.mass = 1;
        playerAnimator.SetBool(BlnAnimSummon, false);
    }

    #endregion

    #region Message Rebind component

    public void RebindComponents()
    {
        // Rebind camera confiner
        GameObject.FindWithTag("MainCamera").GetComponentInChildren<CinemachineConfiner>().m_BoundingShape2D =
            GameObject.FindWithTag("BackGround").GetComponent<Collider2D>();

        Debug.Log("Rebind cam");
    }

    #endregion
}