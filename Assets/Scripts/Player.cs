using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.InputSystem;
using MoreMountains.NiceVibrations;

public class Player : MonoBehaviour
{
    //Config
    [Header("Movement")]
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float sensitivity = 0.5f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] float minFallVelocity = -14.5f;

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

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();

        defaultGravityScale = myRigidbody2D.gravityScale;
        myRigidbody2D.gravityScale = defaultGravityScale;
    }

    void Update()
    {
        if (isAlive)
        {
            CheckIsJumping();   
        }

        myAnimator.SetBool("isOnGround", myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")));
        myAnimator.SetFloat("yVelocity", myRigidbody2D.velocity.y);
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            Run();
            FlipSprite();
            ModifyJump();
        }

        ClampFallSpeed();
    }

    private void Run()
    {
        runThrow = Mathf.MoveTowards(runThrow, inputXValue, sensitivity * Time.fixedDeltaTime);
        print(runThrow);

        Vector2 playerVelocity = new Vector2(runThrow * runSpeed * Time.fixedDeltaTime, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed) MMVibrationManager.Haptic(HapticTypes.Selection);
        else if(context.phase == InputActionPhase.Canceled) MMVibrationManager.Haptic(HapticTypes.LightImpact);

        jumpActionPhase = context.phase;
        Jump();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        inputXValue = context.ReadValue<Vector2>().x;
    }

    public void SetInputXValue(float value)
    {
        inputXValue = value;

        if(inputXValue == 0) MMVibrationManager.Haptic(HapticTypes.LightImpact);
        else MMVibrationManager.Haptic(HapticTypes.Selection);
    }

    public void Jump()
    {
        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if(jumpActionPhase != InputActionPhase.Performed) return;

        isJumping = true;

        jumpFeedbacks.PlayFeedbacks();

        myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0);

        Vector2 jumpVelocityToAdd = new Vector2(0f, jumpForce);
        myRigidbody2D.velocity += jumpVelocityToAdd;
    }

    private void CheckIsJumping()
    {
        if (Mathf.Abs(myRigidbody2D.velocity.y) < Mathf.Epsilon)
        {
            isJumping = false;
        }
    }

    private void ModifyJump()
    {
        if (!isJumping) { return; }

        if (myRigidbody2D.velocity.y < 0)
        {
            myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (myRigidbody2D.velocity.y > 0 && jumpActionPhase != InputActionPhase.Performed)
        {
            myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private void ClampFallSpeed()
    {
        myRigidbody2D.velocity = new Vector2(
            myRigidbody2D.velocity.x,
            Mathf.Clamp(myRigidbody2D.velocity.y, minFallVelocity, Mathf.Infinity));
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }
}
