using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementBehavior : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject head;
    public float checkRadius;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private Rigidbody2D rb;
    private float moveInput;
    private Vector2 moveVelocity;
    private bool facingRight = true;

    [Header("Water Movement")]
    public float Waterspeed;
    private bool InWater = false;

    [Header("Air Movement")]
    public float Landspeed;
    private bool isGrounded;

    [Header("Jump Configuration")]
    public float jumpForce;
    private int extraJumps;
    private int extraJumpsValue;

    [Header("Other")]
    bool Ploof = false;
    public Text DepthM;
  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        checkState();
        JumpSystem();

        Decceleration();
        DepthMeter();

    }

    void FixedUpdate()
    {

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

    void checkState()
    {
        if (InWater)
        {
            WaterMovement();
        }
        else
        {
            LandMovement();
        }
    }

    void WaterMovement()
    {
        //Movement For Water
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = moveInput * Waterspeed;
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

    }
    void LandMovement()
    {
        rb.gravityScale = 2f;
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * Landspeed, rb.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    void JumpSystem()
    {
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
        if (collision.CompareTag("Water") && !InWater)
        {
            InWater = true;
            rb.gravityScale = rb.velocity.magnitude * 2;
            Ploof = true;
            print("In");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //detect if player exited body of water
        if (collision.CompareTag("Water") && InWater)
        {
            InWater = false;
            print("Out");
        }
    }

    void Decceleration()
    {
        if (Ploof)
        {

            rb.gravityScale = rb.gravityScale - 0.045f;

            if (rb.gravityScale <= 0)
            {
                rb.gravityScale = 0;
                Ploof = false;
            }

        }
    }

    void DepthMeter()
    {
        if (transform.position.y > 0)
        {
            DepthM.text = "Depth: 0m";
        } else
        {
            float depth = Mathf.Abs(transform.position.y * 2);
            DepthM.text = "Depth: " + depth.ToString("0") + "m";
        }

    }
}
