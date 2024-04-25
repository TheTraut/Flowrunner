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

    public float waterSwimPower = 3f;  // Adjust as needed
<<<<<<< Updated upstream:Flow Runner/Assets/Scripts/PlayerMovement.cs
    private bool isInWater = false;  // Track if the player is in water
=======
    public bool isInWater = false;  // Track if the player is in water
    public bool isUnderWater = false;  // Track if the player is in water

    public bool shielded;
    public float shieldTime = 2f;
    [SerializeField]
    private GameObject shield;
>>>>>>> Stashed changes:Flow Runner/Assets/Scripts/PlayerScripts/PlayerMovement.cs

    // Start is called before the first frame update
    void Start()
    {
        shielded = false;
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D component not found on the player. Adding Rigidbody2D component...");
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        
        if (!PauseManager.isPaused)
        {
            HandleMovement();
        }
        CheckShield();
    }

    void CheckShield()
    {
        if (Input.GetKey(KeyCode.Space) && !shielded)
        {
            shield.SetActive(true);
            shielded = true;
            Invoke("NoShield", shieldTime);
        }
    }

    void NoShield()
    {
        shield.SetActive(false);
        shielded = false;
    }

    void HandleMovement()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isJumping = true;
            jumpCounter = 0;
        }

        if (rb.velocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime) isJumping = false;

            float tCal = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (tCal > 0.6f)
            {
                currentJumpM = jumpMultiplier * (1 - tCal);
            }

            rb.velocity += vecGravity * currentJumpM * Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpCounter = 0;

            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }

        // Swimming controls
        if (isInWater)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))  // Swim up
            {
                rb.AddForce(new Vector2(0, waterSwimPower), ForceMode2D.Force);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  // Swim down
            {
                rb.AddForce(new Vector2(0, -waterSwimPower), ForceMode2D.Force);
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
            // Set the flag to indicate the player is in water
            isInWater = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // Reset the flag when the player exits water
            isInWater = false;
        }
    }
}
