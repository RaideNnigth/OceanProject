using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehavior : MonoBehaviour
{
    public float Waterspeed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    public bool InWater = false;

    public float Landspeed;
    public float jumpForce;
    private float moveInput;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    public int extraJumpsValue;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = moveInput * Waterspeed;

        CheckStatus();

        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetKeyDown(KeyCode.W) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.W) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        
    }

    void FixedUpdate()
    {
       

        if (InWater)
        {
            rb.gravityScale = 0;
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        } else
        {
            rb.gravityScale = 1f;
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);



            moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * Landspeed, rb.velocity.y);
        }

        

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
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

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}



