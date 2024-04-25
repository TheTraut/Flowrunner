using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb; // Rigidbody2D component for player movement

    [Header("Jump System")]
    public float jumpTime; // Time the player can hold the jump button
    public float jumpPower; // Initial jump velocity
    public float fallMultiplier; // Multiplier for gravity when falling
    public float jumpMultiplier; // Multiplier for gravity when jumping
    public float playerSpeed; // Speed of the player

    public Transform groundCheck; // Transform for checking if the player is grounded
    public LayerMask groundLayer; // Layer mask for detecting ground
    Vector2 vecGravity; // Vector to store gravity

    bool isJumping; // Flag to track if the player is jumping
    float jumpCounter; // Counter for tracking jump time

    public float waterSwimPower = 3f; // Power for swimming in water
    public bool isInWater = false; // Flag to track if the player is in water
    public bool isUnderWater = false; // Flag to track if the player is underwater

    public bool shielded; // Flag to track if the player is shielded
    public float shieldTime = 2f; // Duration of the shield
    [SerializeField]
    private GameObject shield; // Reference to the shield GameObject

    // Start is called before the first frame update
    void Start()
    {
        shielded = false;
        vecGravity = new Vector2(0, -Physics2D.gravity.y); // Initialize gravity vector
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D component not found on the player. Adding Rigidbody2D component...");
            rb = gameObject.AddComponent<Rigidbody2D>(); // Add Rigidbody2D component if not found
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (!PauseManager.isPaused) // Check if the game is not paused
        {
            HandleMovement(); // Handle player movement
        }
        CheckShield(); // Check if the shield is active
    }

    // Check if the shield is active
    void CheckShield()
    {
        if (Input.GetKey(KeyCode.Space) && !shielded) // Check if the space key is pressed and shield is not active
        {
            shield.SetActive(true); // Activate the shield
            shielded = true; // Set shielded flag to true
            Invoke("NoShield", shieldTime); // Schedule the deactivation of the shield
        }
    }

    // Deactivate the shield
    void NoShield()
    {
        shield.SetActive(false); // Deactivate the shield
        shielded = false; // Set shielded flag to false
    }

    // Handle player movement
    void HandleMovement()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded()) // Check if the jump button is pressed and player is grounded
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower); // Apply vertical jump velocity
            isJumping = true; // Set jumping flag to true
            jumpCounter = 0; // Reset jump counter
        }

        if (rb.velocity.y > 0 && isJumping) // Check if the player is moving upwards and jumping
        {
            jumpCounter += Time.deltaTime; // Increment jump counter
            if (jumpCounter > jumpTime) isJumping = false; // Disable jumping if jump time exceeds limit

            float tCal = jumpCounter / jumpTime; // Calculate normalized time for jump
            float currentJumpM = jumpMultiplier; // Initialize current jump multiplier

            if (tCal > 0.6f) // Adjust jump multiplier based on time
            {
                currentJumpM = jumpMultiplier * (1 - tCal);
            }

            rb.velocity += vecGravity * currentJumpM * Time.deltaTime; // Apply gravity with adjusted multiplier
        }

        if (Input.GetButtonUp("Jump")) // Check if the jump button is released
        {
            isJumping = false; // Set jumping flag to false
            jumpCounter = 0; // Reset jump counter

            if (rb.velocity.y > 0) // Check if the player is moving upwards
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // Reduce vertical velocity
            }
        }

        if (rb.velocity.y < 0) // Check if the player is moving downwards
        {
            rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime; // Apply increased gravity when falling
        }

        // Swimming controls
        if (isInWater) // Check if the player is in water
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))  // Swim up
            {
                rb.AddForce(new Vector2(0, waterSwimPower), ForceMode2D.Force); // Apply upward force for swimming
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  // Swim down
            {
                rb.AddForce(new Vector2(0, -waterSwimPower), ForceMode2D.Force); // Apply downward force for swimming
            }
        }
    }

    // Check if the player is grounded
    public bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1, 0.03f), CapsuleDirection2D.Horizontal, 0, groundLayer); // Check for ground overlap
    }

    // Property to get if the player is in water
    public bool IsInWater
    {
        get { return isInWater; }
    }

    // Property to get if the player is underwater
    public bool IsUnderWater
    {
        get { return isUnderWater; }
    }

    // Trigger enter event for water
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water")) // Check if the collider is tagged as water
        {
            // Set the flag to indicate the player is in water
            isInWater = true;
        }
    }

    // Trigger exit event for water
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water")) // Check if the collider is tagged as water
        {
            // Reset the flag when the player exits water
            isInWater = false;
        }
    }
}