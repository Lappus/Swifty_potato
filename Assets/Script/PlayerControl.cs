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
    public bool reachedGoal = false; // Flag to check if the player has reached the goal
    private bool invicible = false; // Flag to indicate if the player is invincible
    
    private HandleHeadUpDisplay handleHeadUpDisplay; // Head up display for displacing lives left

    [Header("For Movement")]
    [SerializeField] public float moveSpeed = 4f;
    [SerializeField] private float groundDrag;
    private float xDirectionalInput; // Holds input for left/right movement
    private bool facingRight = true;
    private Vector3 movement;

    [Header("For Jumping")] 
    [SerializeField] float jumpForce = 8f; // Force applied for jumping
    [SerializeField] LayerMask groundLayer; // Layer to check for ground collisions
    [SerializeField] Transform groundCheckPoint; // Point used to check if the player is grounded
    [SerializeField] private float groundRadius;
    [SerializeField] private float jumpCooldown = 1f;
    [SerializeField] private float airMultiplier;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool pressedJump;

    [Header("For WallSliding")] 
    //[SerializeField] float wallSlideSpeed = 0f;
    [SerializeField] LayerMask wallLayer; // Layer to check for wall collisions
    [SerializeField] Transform wallCheckPoint; // Point used to check if the player is against a wall
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
        // Process inputs, check environment conditions, control speed, and handle wall jumping
        ProcessInputs();
        CheckEnvironment();
        SpeedControl();
        WallJump();
        
        // Apply ground drag if grounded, otherwise no drag
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
        lives = initialLives; // Set initial lives
        RB = GetComponent<Rigidbody>();

        // Set initial position and controls based on player number
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
        xDirectionalInput = Input.GetAxisRaw(horizontalAxis); // Get horizontal input
        pressedJump = Input.GetKeyDown(jumpKey); // Check if jump key is pressed
        
        // Handle jumping if grounded and can jump
        if (isGrounded && canJump && pressedJump)
        {
            canJump = false;
            Jump();
            animator.SetBool("isJumping", true); // Trigger jump animation
            Invoke(nameof(ResetJump), jumpCooldown); // Reset jump after cooldown
        }
    
        // Update jump animation based on grounded/wall status
        if (!isGrounded && !isWall)
        {
            animator.
            SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    void Jump()
    {
        RB.velocity = new Vector3(RB.velocity.x, 0f, 0f); // Reset vertical velocity
        RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // Apply jump force
    }

    void ResetJump()
    {
        canJump = true; // Allow jumping again
    }

    void CheckEnvironment()
    {
        // Check if the player is grounded or against a wall
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundRadius, groundLayer);
        isWall = Physics.CheckSphere(wallCheckPoint.position, wallRadius, wallLayer);
    }

    void Move()
    {
        // Stop movement if the player has reached the goal
        if (reachedGoal)
        {
            RB.velocity = Vector3.zero;
            return;
        }
            
        movement = new Vector3(xDirectionalInput, 0f, 0f);
        
        // Trigger running animation if moving
        if (xDirectionalInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        
        // Apply movement force based on grounded/air status
        if (isGrounded)
        {
            RB.AddForce(movement.normalized * moveSpeed * 5f, ForceMode.Force);
        }
        else
        {
            RB.AddForce(movement.normalized * moveSpeed * 5f * airMultiplier, ForceMode.Force);
        }
    
        // Flip the player if moving in the opposite direction
        if (xDirectionalInput < 0 && facingRight || xDirectionalInput > 0 && !facingRight)
        {
            Flip();
        }
        
        // Handle losing a life if the player falls below a certain height
        if (transform.position.y < -13f)
        {
            LosingLive();
        }

    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Handle losing a life if hit by a weapon
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
        lives--; // Decrease lives
        
        // Reset position and update HUD based on player number
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
        StartCoroutine(HandleInvincibility()); // Handle invincibility after losing a life
        IsDead(); // Check if the player is dead
        
    }
    
    private void IsDead()
    {
        // Handle player death if lives are depleted
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

            DeactivatePlayer(); // Deactivate the player
            if (gameManager != null)
            {
                gameManager.CheckGameState(); // Check the game state if phase has to be changed because both player are either dead or in goal
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
            //RB.velocity = new Vector3(0, wallSlideSpeed, 0f); 
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
        // Reset the player's state
        if (RB == null) RB = GetComponent<Rigidbody>();
        
        // Reset position based on player number
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