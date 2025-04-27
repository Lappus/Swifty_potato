using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Player Status")]
    public int playerNumber; // 1 for Player One, 2 for Player Two
    public int initialLives = 3;
    private GameManager gameManager;

    [Header("Goal / Life Status")]
    public static bool playerOneIsDead = false;
    public static bool playerTwoIsDead = false;
    public static bool playerOneReachedGoal = false;
    public static bool playerTwoReachedGoal = false;
    public static bool playerOneTurnIsOver = true;
    public static bool playerTwoTurnIsOver = true;
    private int lives;
    public bool reachedGoal = false;
    private bool invicible = false;
    
    // UI
    private HandleHeadUpDisplay handleHeadUpDisplay;

    [Header("For Movement")]
    [SerializeField] public float moveSpeed = 4f;
    [SerializeField] private float groundDrag;
    private float xDirectionalInput;
    private bool facingRight = true;
    private Vector3 movement;

    [Header("For Jumping")] 
    [SerializeField] float jumpForce = 8f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] private float groundRadius;
    [SerializeField] private float jumpCooldown = 1f;
    [SerializeField] private float airMultiplier;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool pressedJump;

    [Header("For WallSliding")] 
    [SerializeField] float wallSlideSpeed = 0f;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] private float wallRadius;
    [SerializeField] private bool isWall;
    [SerializeField] private bool isWallSliding;
    
    [Header("For WallJump")] 
    [SerializeField] float wallJumpForce = 6f;
    [SerializeField] Vector3 wallJumpAngle;
    private float wallJumpDirection = -1;
    
    [Header("Others")]
    private Rigidbody RB;
    private string horizontalAxis;
    private KeyCode jumpKey;

    [Header("Animation")] 
    [SerializeField] private Animator animator;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        handleHeadUpDisplay = FindObjectOfType<HandleHeadUpDisplay>();
        
        // Initial player setup
        InitPlayer();
        canJump = true;
    }

    void Update()
    {
        ProcessInputs();
        CheckEnvironment();
        SpeedControl();
        WallJump();

        if (isGrounded)
        {
            RB.drag = groundDrag;
        }
        else
        {
            RB.drag = 0;
        }
    }

    void FixedUpdate()
    {
        Move();
        WallSlide();
        //WallJump();
    }

    void InitPlayer()
    {
        lives = initialLives;
        RB = GetComponent<Rigidbody>();

        if (playerNumber == 1)
        {
            transform.position = new Vector3(8f, 4.5f, 0f);
            horizontalAxis = "Horizontal";
            jumpKey = KeyCode.W;
        }
        else
        {
            transform.position = new Vector3(12f, 4.5f, 0f);
            horizontalAxis = "Horizontal2";
            jumpKey = KeyCode.UpArrow;
        }
    }
    void ProcessInputs()
    {
        xDirectionalInput = Input.GetAxisRaw(horizontalAxis);
        pressedJump = Input.GetKeyDown(jumpKey);
        
        if (isGrounded && canJump && pressedJump)
        {
            canJump = false;
            Jump();
            animator.SetBool("isJumping", true);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    void Jump()
    {
        RB.velocity = new Vector3(RB.velocity.x, 0f, 0f);
        RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // RB.AddForce(transform.up * jumpForce, ForceMode.Impulse)
    }

    void ResetJump()
    {
        canJump = true;
    }

    void CheckEnvironment()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundRadius, groundLayer);
        isWall = Physics.CheckSphere(wallCheckPoint.position, wallRadius, wallLayer);
    }

    void Move()
    {
        if (reachedGoal)
        {
            RB.velocity = Vector3.zero;
            //if (isGrounded && canJump)
            //{
            //    canJump = false;
            //    Jump();
            //   Invoke(nameof(ResetJump), jumpCooldown);
            //}
            return;
        }
            
        movement = new Vector3(xDirectionalInput, 0f, 0f);
        
        if (xDirectionalInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (isGrounded)
        {
            RB.AddForce(movement.normalized * moveSpeed * 5f, ForceMode.Force);
        }
        else
        {
            RB.AddForce(movement.normalized * moveSpeed * 5f * airMultiplier, ForceMode.Force);
        }

        if (xDirectionalInput < 0 && facingRight || xDirectionalInput > 0 && !facingRight)
        {
            Flip();
        }

        if (transform.position.y < -13f)
        {
            LosingLive();
        }

    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Überprüfen, ob das Objekt mit dem Tag "weapon" ist
        if (other.CompareTag("weapon") && other.gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            if (!invicible)
            {
                LosingLive();
            }
        }
    }
    private void LosingLive()
    {
        lives--;
        
        if (playerNumber == 1)
        {
            transform.position = new Vector3(8f, 4.5f, 0f);
            handleHeadUpDisplay.HandleLiveUIPlayerOne(lives);
        }   
        else if (playerNumber == 2)
        {
            transform.position = new Vector3(12f, 4.5f, 0f);
            handleHeadUpDisplay.HandleLiveUIPlayerTwo(lives);
        }
        
        Debug.Log($"Player {playerNumber} - Lives Left: {lives}");
        StartCoroutine(HandleInvincibility());
        IsDead();
        
    }
    
    private void IsDead()
    {
        if (lives < 1)
        {
            transform.position = new Vector3(0, 2, 0);
            if (playerNumber == 1)
            {
                playerOneTurnIsOver = true;
                playerOneIsDead = true;
            }
            else if (playerNumber == 2)
            {
                playerTwoTurnIsOver = true;
                playerTwoIsDead = true;
            }

            DeactivatePlayer();
            if (gameManager != null)
            {
                gameManager.CheckGameState();
            }
        }
    }
    
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(RB.velocity.x, 0f, 0f);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            RB.velocity = new Vector3(limitedVel.x, RB.velocity.y);
        }
    }
    void Flip()
    {
        wallJumpDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }
    
    void WallSlide()
    {
        if (isWall && !isGrounded)
        {
            isWallSliding = true;
            //RB.velocity = new Vector3(0, wallSlideSpeed, 0f); //führt zu Fehlern!!!!!
        }
        else
        {
            isWallSliding = false;
        }
    }

    void WallJump()
    {
        if ((isWallSliding) && canJump && pressedJump)
        {
            RB.velocity = new Vector3(0f, 0f, 0f);
            RB.AddForce(new Vector3(wallJumpForce*wallJumpDirection*wallJumpAngle.x,wallJumpForce*wallJumpAngle.y), ForceMode.Impulse);
            canJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void DeactivatePlayer()
    {
        gameObject.SetActive(false);
    }

    void ActivatePlayer()
    {
        gameObject.SetActive(true);
        invicible = false;
    }

    public void ResetPlayer()
    {
        print("reset was done");
        print($"Mal gucken reached goal: {reachedGoal}");
        if (RB == null) RB = GetComponent<Rigidbody>();

        if (playerNumber == 1)
        {
            transform.position = new Vector3(8f, 4.5f, 0f);
        }
        else
        {
            transform.position = new Vector3(12f, 4.5f, 0f);
        }

        lives = initialLives;
        
        ActivatePlayer();
        reachedGoal = false;
        playerOneIsDead = false;
        playerTwoIsDead = false;
        playerOneReachedGoal = false;
        playerTwoReachedGoal = false;
        playerOneTurnIsOver = false;
        playerTwoTurnIsOver = false;
        print($"Mal gucken zwei: {reachedGoal}");
        RB.isKinematic = false;
    }

    private IEnumerator HandleInvincibility()
    {
        invicible = true;
        print("Now invincible");
        yield return new WaitForSeconds(0.5f);
        invicible = false;
        print("Now killable");
    }
    
}