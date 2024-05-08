using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb; // Rigidbody2D component for player movement

    [SerializeField] private AudioClip splashSoundClip;
    [SerializeField] private AudioClip shieldSoundClip;
    [SerializeField] private AudioClip jumpSoundClip;

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
    public float shieldTime = 5f; // Duration of the shield
    readonly float shieldCooldownTime = 10f;
    bool canUseShield = true; // Flag to track if the shield can be used
    Coroutine shieldCooldownCoroutine; // Coroutine reference for shield cooldown
    float remainingShieldTime;
    float pausedTimeRemainingShield; // Store the remaining shield time when paused
    Coroutine shieldCoroutine;
    #pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] private GameObject shield; // Reference to the shield GameObject
    #pragma warning restore IDE0044 // Add readonly modifier

    /// <summary>
    /// Handles player movement, including jumping, gravity adjustments, and swimming.
    /// </summary>
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

    /// <summary>
    /// Updates player movement based on input and game state.
    /// </summary>
    public void Update()
    {
        if (!PauseManager.IsPaused) // Check if the game is not paused
        {
            HandleMovement(); // Handle player movement
            CheckShield(); // Check if the shield is active
        }
    }

    /// <summary>
    /// Checks if the shield is active and deactivates it after a set duration.
    /// </summary>
    void CheckShield()
    {
        // Check if the game is not paused, and the shield key combination is pressed, and the shield is not active
        if (!PauseManager.IsPaused && SettingsManagerExtensions.AreKeyCombinationsPressed(SettingsManager.Instance.ShieldShortcutKeys) && !shielded && canUseShield)
        {
            SoundFXManager.instance.PlaySoundFXClip(shieldSoundClip, transform);
            shield.SetActive(true); // Activate the shield
            StartShieldCooldown(); // Start the shield cooldown coroutine
            if (remainingShieldTime <= 0f)
            {
                remainingShieldTime = shieldTime;
            }
            if (shieldCoroutine != null)
            {
                StopCoroutine(shieldCoroutine);
            }
            shieldCoroutine = StartCoroutine(NoShieldCoroutine(remainingShieldTime));
            shielded = true;
        }
    }

    /// <summary>
    /// Starts the cooldown for using the shield.
    /// </summary>
    void StartShieldCooldown()
    {
        canUseShield = false; // Disable shield usage
        if (shieldCooldownCoroutine != null)
        {
            StopCoroutine(shieldCooldownCoroutine); // Stop previous cooldown coroutine if exists
        }
        shieldCooldownCoroutine = StartCoroutine(ShieldCooldown());
    }

    /// <summary>
    /// Coroutine for the shield cooldown.
    /// </summary>
    IEnumerator ShieldCooldown()
    {
        float remainingShieldCooldownTime = shieldCooldownTime; // Initial cooldown time
        while (remainingShieldCooldownTime > 0f)
        {
            if (!PauseManager.IsPaused)
            {
                remainingShieldCooldownTime -= Time.deltaTime; // Decrease remaining cooldown time
            }
            yield return null;
        }
        canUseShield = true; // Enable shield usage after cooldown
    }

    /// <summary>
    /// Deactivates the shield.
    /// </summary>
    IEnumerator NoShieldCoroutine(float delay)
    {
        float remainingTime = delay;

        while (remainingTime > 0f)
        {
            if (!PauseManager.IsPaused)
            {
                remainingTime -= Time.deltaTime;
            }
            else
            {
                pausedTimeRemainingShield = remainingTime;
                yield return null;
            }
            yield return null;
        }

        shield.SetActive(false);
        shielded = false;

        // Check if there's remaining time when unpaused
        if (pausedTimeRemainingShield > 0f)
        {
            remainingTime = pausedTimeRemainingShield;
            pausedTimeRemainingShield = 0f;
            while (remainingTime > 0f)
            {
                if (!PauseManager.IsPaused)
                {
                    remainingTime -= Time.deltaTime;
                }
                yield return null;
            }
            shield.SetActive(false);
            shielded = false;
        }
    }

    /// <summary>
    /// Handles player movement including jumping, gravity adjustments, and swimming controls.
    /// </summary>
    void HandleMovement()
    {
        // Check if the jump button is pressed and player is grounded
        if (SettingsManagerExtensions.AreKeyCombinationsPressed(SettingsManager.Instance.UpShortcutKeys) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower); // Apply vertical jump velocity
            isJumping = true; // Set jumping flag to true
            jumpCounter = 0; // Reset jump counter
        }

        // Check if the jump button is released
        if (!SettingsManagerExtensions.AreKeyCombinationsPressed(SettingsManager.Instance.UpShortcutKeys) && isJumping)
        {
            isJumping = false; // Set jumping flag to false
            jumpCounter = 0; // Reset jump counter

            if (rb.velocity.y > 0) // Check if the player is moving upwards
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // Reduce vertical velocity
            }
        }

        // Check if the player is moving upwards and jumping
        if (rb.velocity.y > 0 && isJumping)
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

        // Check if the player is moving downwards
        if (rb.velocity.y < 0)
        {
            rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime; // Apply increased gravity when falling
        }

        // Swimming controls
        if (isInWater) // Check if the player is in water
        {
            if (SettingsManagerExtensions.AreKeyCombinationsPressed(SettingsManager.Instance.UpShortcutKeys))  // Swim up
            {
                rb.AddForce(new Vector2(0, waterSwimPower), ForceMode2D.Force); // Apply upward force for swimming
            }
            else if (SettingsManagerExtensions.AreKeyCombinationsPressed(SettingsManager.Instance.DownShortcutKeys))  // Swim down
            {
                rb.AddForce(new Vector2(0, -waterSwimPower), ForceMode2D.Force); // Apply downward force for swimming
            }
        }
    }

    /// <summary>
    /// Checks if the player is grounded.
    /// </summary>
    /// <returns>True if the player is grounded, otherwise false.</returns>
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

    /// <summary>
    /// Trigger enter event for water collision.
    /// </summary>
    /// <param name="other">The Collider2D object the player has collided with.</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water")) // Check if the collider is tagged as water
        {
            SoundFXManager.instance.PlaySoundFXClip(splashSoundClip, transform);
            // Set the flag to indicate the player is in water
            isInWater = true;
        }
    }

    /// <summary>
    /// Trigger exit event for water collision.
    /// </summary>
    /// <param name="other">The Collider2D object the player has exited from.</param>
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water")) // Check if the collider is tagged as water
        {
            // Reset the flag when the player exits water
            isInWater = false;
        }
    }
}