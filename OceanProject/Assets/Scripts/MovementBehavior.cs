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

    public GameObject head;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        //Movement For Water
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = moveInput * Waterspeed;

        //Check if on platform
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        //Jump Script (maybe double jump)
        if (Input.GetKeyDown(KeyCode.W) && extraJumps > 0 || Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.W) && extraJumps == 0 && isGrounded == true || Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        
    }

    void FixedUpdate()
    {
        //Check if inside water or outside
        if (InWater)
        {
            rb.gravityScale = 1.5f;
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        } else
        {
            rb.gravityScale = 2f;
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);



            moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * Landspeed, rb.velocity.y);
        }

        
        //flip sprite when going left or right
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

    }

   

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //detect if player entered body of water
        if (collision.CompareTag("Water"))
        {
            InWater = true;
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //detect if player exited body of water
        if (collision.CompareTag("Water"))
        {
            InWater = false;
            
        }
    }
}



