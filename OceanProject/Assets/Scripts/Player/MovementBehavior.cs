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
    public float DashMultipler;
    public float DashDelay;
    private bool InWater = false;
    private bool canDash = true;

    [Header("Air Movement")]
    public float Landspeed;
    private bool isGrounded;

    [Header("Jump Configuration")]
    public float jumpForce;
    private int extraJumps;
    private int extraJumpsValue;

    [Header("Other")]
    bool HeadinWater;
    bool Ploof = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HeadinWater = OxygenLogic.HeadinWater;
    }

    void Update()
    {
        HeadinWater = OxygenLogic.HeadinWater;
        checkState();
        JumpSystem();
        Decceleration();
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
        if (HeadinWater)
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            moveVelocity = moveInput * Waterspeed * Time.deltaTime * 20;

            if (Input.GetKeyDown(KeyCode.Space) && canDash)
            {
                moveVelocity *= DashMultipler;
                rb.velocity = moveVelocity;
                StartCoroutine(waitToDash());
            }
            else
            {
                rb.velocity = moveVelocity;
            }
        }
        else
        {
            //IMPLEMENT HERE THE HALF BODY CODE 
        }
    }
    void LandMovement()
    {
        rb.gravityScale = 2f;
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = (new Vector2(moveInput * Landspeed, rb.velocity.y));
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        //detect if player entered body of water
        if (collision.CompareTag("Water") && !InWater)
        {
            InWater = true;
            rb.gravityScale = rb.velocity.magnitude * 2;
            Ploof = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //detect if player exited body of water
        if (collision.CompareTag("Water") && InWater)
        {
            InWater = false;
        }
    }

    IEnumerator waitToDash()
    {
        canDash = false;
        yield return new WaitForSeconds(DashDelay);
        canDash = true;
    }
    
}
