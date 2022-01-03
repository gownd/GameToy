using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerCollider : MonoBehaviour
{
    [SerializeField] float moveSpeed = 800f;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0f;

            Vector2 distance = (Vector2)touchPosition - rb.position;
            rb.velocity = distance * Time.deltaTime * moveSpeed;
        }
    }

    // private void OnCollisionEnter2D(Collision2D other) {
    //     print(other.gameObject.name);
    // }
}
