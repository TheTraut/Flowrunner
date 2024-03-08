using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Jump System")]
    public float jumpTime;
    public float jumpPower;
    public float fallMultiplier;
    public float jumpMultiplier;
    public float playerSpeed;

    public Transform groundCheck;
    public LayerMask groundLayer;
    Vector2 vecGravity;

    bool isJumping;
    float jumpCounter;

    // Water physics variables
    private float originalJumpPower;
    private float originalGravityScale;
    private float originalfallMultiplier;
    public float waterJumpPower = 2f;  // Adjust as needed
    public float waterGravityScale = 0.2f;  // Adjust as needed
    private bool isInWater = false;  // Track if the player is in water

    // Start is called before the first frame update
    void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D component not found on the player. Adding Rigidbody2D component...");
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Save original physics properties
        originalJumpPower = jumpPower;
        originalGravityScale = rb.gravityScale;
        originalfallMultiplier = fallMultiplier;
    }

    // Update is called once per frame
    public void Update()
    {

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            //rb.velocity = Vector2.right * playerSpeed;
            isJumping = true;
            jumpCounter = 0;
        }

        if(rb.velocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime) isJumping = false;

            float tCal = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if(tCal > 0.6f)
            {
                currentJumpM = jumpMultiplier * (1 - tCal);
            }

            rb.velocity += vecGravity * currentJumpM * Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpCounter = 0;

            if(rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

        if(rb.velocity.y < 0)
        {
            rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }

        // Swimming controls
        if (isInWater)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))  // Swim up
            {
                rb.velocity = new Vector2(rb.velocity.x, waterJumpPower);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  // Swim down
            {
                rb.velocity = new Vector2(rb.velocity.x, -waterJumpPower);
            }
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1, 0.03f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    public bool IsInWater
    {
        get { return isInWater; }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // Apply water physics properties
            jumpPower = waterJumpPower;
            rb.gravityScale = waterGravityScale;
            fallMultiplier = fallMultiplier * waterGravityScale;
            isInWater = true;  // Set the flag to indicate the player is in water
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // Revert to original physics properties
            jumpPower = originalJumpPower;
            rb.gravityScale = originalGravityScale;
            fallMultiplier = originalfallMultiplier;
            // Add a small boost to the player's momentum
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 3f); // Adjust the value as needed
            isInWater = false;  // Reset the flag when the player exits water
        }
    }
}
