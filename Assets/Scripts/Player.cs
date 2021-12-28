﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.InputSystem;

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

    [Header("Feedbacks")]
    [SerializeField] MMFeedbacks jumpFeedbacks = null;

    //State
    bool isAlive = true;
    bool isJumping = false;
    float defaultGravityScale;

    //Cached Componenet References
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;
    Animator myAnimator;

    //Control
    InputActionPhase jumpActionPhase;
    float inputXValue;
    float runThrow;
    float coyoteTimeCounter;
    float jumpBufferCounter;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();

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

        ClampFallSpeed();
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
    }

    void SetRunThrow()
    {
        float sensitivity = 0f;

        if(isJumping)
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

        jumpFeedbacks.PlayFeedbacks();
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
        // if (!isJumping) { return; }

        if (myRigidbody2D.velocity.y < 0)
        {
            myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (myRigidbody2D.velocity.y > 0 && jumpActionPhase != InputActionPhase.Performed)
        {
            myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
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
        return myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void UpdateAnimatorParameters()
    {
        myAnimator.SetBool("isOnGround", IsGrounded());
        myAnimator.SetFloat("yVelocity", myRigidbody2D.velocity.y);
    }
}
