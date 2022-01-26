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

    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Animator playerAnimator;
    public Collider2D bodyCollider;
    public Collider2D groundDetectCollider;
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
        _isJump = false;
        _isBuilding = false;
        _isHurt = false;
        _isPause = false;

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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield break;
    }

    #endregion

    #region Move

    private void Move()
    {
        // Face direction
        var rawHorizontal = Input.GetAxisRaw("Horizontal");
        if (rawHorizontal != 0)
        {
            playerRigidbody.transform.localScale = new Vector3(rawHorizontal * 0.02f, 0.02f, 1);
            // Anim run trigger
            playerAnimator.SetBool(BlnAnimRun, true);
        }
        else
        {
            playerAnimator.SetBool(BlnAnimRun, false);
        }

        if (groundDetectCollider.IsTouchingLayers(ground))
        {
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

        _isJump = true;
    }

    private void ExecuteJump()
    {
        if (!_isJump) return;
        _isJump = false;

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
}