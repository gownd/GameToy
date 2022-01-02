using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Config
    [Header("Movement")]
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float sensitivity_a = 0.5f;
    [SerializeField] float sensitivity_r = 0.5f;
    [SerializeField] float sensitivity_jump = 0.1f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] float minFallVelocity = -14.5f;
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.1f;
    [SerializeField] float enemyJumpMultiplier = 1.1f;

    //State
    bool isAlive = true;
    bool isJumping = false;
    bool longJumpCheck = true;
    bool enemyJump = false;
    float defaultGravityScale;

    //Cached Componenet References
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;
    Animator myAnimator;

    CollisionSideDetector collisionSideDetector;

    //Control
    InputActionPhase jumpActionPhase;
    float inputXValue;
    float runThrow;
    float coyoteTimeCounter;
    float jumpBufferCounter;

    private void Awake() 
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();

        collisionSideDetector = GetComponent<CollisionSideDetector>();
    }

    void Start()
    {
        defaultGravityScale = myRigidbody2D.gravityScale;
        myRigidbody2D.gravityScale = defaultGravityScale;

        AddControls();
    }

    void Update()
    {
        if (isAlive)
        {
            ModifyJumpControl();
            Jump();
            Die();
        }

        UpdateAnimatorParameters();
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            Run();
            FlipSprite();

            HandleShortJump();
        }

        ModifyFall();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isAlive) return;

        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            StartCoroutine(Die());

            // Vector2 bouncVelocity = new Vector2(myRigidbody2D.velocity.x, bounceForce);
            // myRigidbody2D.velocity = bouncVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAlive) return;

        StompEnemy(other);
    }

    private void StompEnemy(Collider2D other)
    {
        if (myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            longJumpCheck = false;
            other.GetComponent<Goomba>().Die();

            longJumpCheck = (jumpBufferCounter >= 0f);

            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0);

            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpForce * enemyJumpMultiplier);
            myRigidbody2D.velocity += jumpVelocityToAdd;
        }
    }

    void AddControls()
    {
        GametoyController gametoyController = FindObjectOfType<GametoyController>();

        gametoyController.HandlePressB += HandleJump;
        gametoyController.HandlePressArrow += HandleMovement;
    }

    void HandleMovement(Vector2 inputValue, InputActionPhase phase)
    {
        SetInputXValue(inputValue.x);
    }

    public void SetInputXValue(float value)
    {
        inputXValue = value;
    }

    void HandleJump(InputAction.CallbackContext context)
    {
        jumpActionPhase = context.phase;
        if (jumpActionPhase == InputActionPhase.Performed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    private void Run()
    {
        SetRunThrow();

        Vector2 playerVelocity = new Vector2(runThrow * runSpeed * Time.fixedDeltaTime, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        myAnimator.speed = 1f * Mathf.Abs(runThrow);

        Skid();
    }

    void Skid()
    {
        if(isJumping) return;

        myAnimator.SetBool("isSkidding", runThrow * inputXValue < 0f);
    }

    void SetRunThrow()
    {
        float sensitivity = 0f;

        if (isJumping)
        {
            sensitivity = sensitivity_jump;
        }
        else
        {
            if (inputXValue == 0)
            {
                sensitivity = sensitivity_r;
            }
            else
            {
                sensitivity = sensitivity_a;
            }
        }

        runThrow = Mathf.MoveTowards(runThrow, inputXValue, sensitivity * Time.fixedDeltaTime);
    }

    public void Jump()
    {
        if (coyoteTimeCounter < 0f || jumpBufferCounter < 0f) return;

        isJumping = true;
        coyoteTimeCounter = 0f;

        myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0);

        Vector2 jumpVelocityToAdd = new Vector2(0f, jumpForce);
        myRigidbody2D.velocity += jumpVelocityToAdd;
    }

    void ModifyJumpControl()
    {
        HandleJumpEnd();
        HandleCoyoteTime();
        HandleJumpBuffer();
    }

    void HandleJumpEnd()
    {
        if (IsGrounded())
        {
            isJumping = false;
            longJumpCheck = true;
        }
    }

    void HandleCoyoteTime()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void HandleJumpBuffer()
    {
        jumpBufferCounter -= Time.deltaTime;
    }

    private void HandleShortJump()
    {
        if (myRigidbody2D.velocity.y > 0)
        {
            if(jumpActionPhase != InputActionPhase.Performed || !longJumpCheck)
            {
                myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
            }
        }
    }

    void ModifyFall()
    {
        if (myRigidbody2D.velocity.y < 0)
        {
            myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;

        if (!isJumping)
        {
            if (inputXValue == 0f) return;

            transform.localScale = new Vector2(inputXValue, 1f);
        }
        else
        {
            if (playerHasHorizontalSpeed)
            {
                transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
            }
        }
    }

    private void ClampFallSpeed()
    {
        myRigidbody2D.velocity = new Vector2(
            myRigidbody2D.velocity.x,
            Mathf.Clamp(myRigidbody2D.velocity.y, minFallVelocity, Mathf.Infinity));
    }

    bool IsGrounded()
    {
        bool isGrounded = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Block"));

        return isGrounded;
    }

    void UpdateAnimatorParameters()
    {
        myAnimator.SetBool("isOnGround", IsGrounded());
        myAnimator.SetFloat("yVelocity", myRigidbody2D.velocity.y);
    }

    public IEnumerator Die()
    {
        if(!isAlive) yield break;

        isAlive = false;

        // isJumping = false;

        myAnimator.SetBool("hasDied", true);
        myBodyCollider2D.isTrigger = true;
        myRigidbody2D.AddForce(new Vector2(0f, 300f));

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // StartCoroutine(FindObjectOfType<GameSession>().ProcessPlayerDeath());
    }
}
