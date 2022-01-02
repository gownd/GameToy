using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    Rigidbody2D rb;
    Vector2 direction;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    private void FixedUpdate() 
    {
        print(direction);
        rb.velocity = direction * Time.deltaTime * moveSpeed;   
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.transform == transform.parent) return;

        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.StartCoroutine(player.Die());
        }

        Destroy(gameObject);    
    }

    public void SetDirection(float angle)
    {
        print(angle);

        if(angle == 0f) // up
        {
            direction = Vector2.up;
            transform.localScale = new Vector3(1f, -1f, 1f);
        } 
        else if(angle == 180f) // down
        {
            direction = Vector2.down;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(angle == 90f) // left
        {
            direction = Vector2.left;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } 
        else if(angle == -90f || angle == 270f) // right
        {
            direction = Vector2.right;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
