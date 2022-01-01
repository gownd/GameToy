using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBlock : MonoBehaviour
{
    [SerializeField] float spinCount = 1f;

    BoxCollider2D boxCollider;
    Animator animator;

    CollisionSideDetector collisionSideDetector;

    private void Awake() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        collisionSideDetector = GetComponent<CollisionSideDetector>();    
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        CollisionSide side = collisionSideDetector.GetCollisionSide(other, boxCollider);    
        if(side == CollisionSide.down)
        {
            StartCoroutine(Spin(other));
        }
    }

    IEnumerator Spin(Collision2D other)
    {
        if(!other.collider.CompareTag("Player")) yield break;

        animator.SetBool("isSpinning", true);
        boxCollider.enabled = false;

        float timeToSpin = 2/3f * spinCount;
        yield return new WaitForSeconds(timeToSpin);

        animator.SetBool("isSpinning", false);
        boxCollider.enabled = true;
    }
}
