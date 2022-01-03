using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitch : Switch
{
    Animator animator;
    BoxCollider2D boxCollider;
    GameObject currentCollision;

    CollisionSideDetector sideDetector;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        sideDetector = GetComponent<CollisionSideDetector>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isOn) return;

        if (sideDetector.GetCollisionSide(other, boxCollider) == CollisionSide.up)
        {
            isOn = true;
            animator.SetBool("isOn", isOn);
            currentCollision = other.gameObject;

            HandleSwitchOn?.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!isOn || other.gameObject != currentCollision) return;

        isOn = false;
        animator.SetBool("isOn", isOn);
        currentCollision = null;

        HandleSwitchOff?.Invoke();
    }
}
