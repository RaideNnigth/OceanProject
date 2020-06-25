using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehavior : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    public bool InWater = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = moveInput * speed;

        CheckStatus();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        if (InWater)
        {
            rb.gravityScale = 0;
        } else
        {
            rb.gravityScale = 9.8f;
        }
        
    }

    public void CheckStatus()
    {
        if (transform.position.y < 0)
        {
            InWater = true;
        } else
        {
            InWater = false;
        }
    }
}
