using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn = null;

    BoxCollider2D boxCollider;
    Animator animator;
    bool hasSpawned = false;

    CollisionSideDetector collisionSideDetector;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        collisionSideDetector = GetComponent<CollisionSideDetector>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasSpawned) return;

        CollisionSide side = collisionSideDetector.GetCollisionSide(other, boxCollider);
        if (side == CollisionSide.down)
        {
            StartCoroutine(SpawnObject(other));
        }
    }

    IEnumerator SpawnObject(Collision2D other)
    {
        if (!(other.collider.CompareTag("Player") || other.collider.CompareTag("Touch"))) yield break;

        hasSpawned = true;

        animator.SetTrigger("Hit");

        objectToSpawn.SetActive(true);
    }
}
