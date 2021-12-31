using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] ObjectActivator activator = null;

    Rigidbody2D myRigidBody2D;
    BoxCollider2D myBoxCollider2D;
    bool isAlive = true;

    private void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if(!activator.IsActive()) return;

        myRigidBody2D.velocity = new Vector2(moveSpeed * Time.deltaTime * transform.localScale.x, myRigidBody2D.velocity.y);
    }

    // private void OnCollisionEnter2D(Collision2D other) 
    // {
    //     if(other.collider.CompareTag("Player"))
    //     {
    //         StartCoroutine(other.collider.GetComponent<Player>().Die());
    //     }    
    // }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>()) return;

        transform.localScale = new Vector2(-transform.localScale.x, 1f);
    }

    public void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return isAlive;
    }
}