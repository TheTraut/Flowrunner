using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Jump System")]
    [SerializeField] float jumpTime;
    [SerializeField] float jumpPower;
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpMultiplier;
    [SerializeField] float playerSpeed;

    public Transform groundCheck;
    public LayerMask groundLayer;
    Vector2 vecGravity;

    bool isJumping;
    float jumpCounter;

    // Water physics variables
    private float originalJumpPower;
    private float originalGravityScale;
    private float originalfallMultiplier;
    public float waterJumpPower = 0.5f;  // Adjust as needed
    public float waterGravityScale = 0.2f;  // Adjust as needed

    // Start is called before the first frame update
    void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        rb = GetComponent<Rigidbody2D>();

        // Save original physics properties
        originalJumpPower = jumpPower;
        originalGravityScale = rb.gravityScale;
        originalfallMultiplier = fallMultiplier;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Jump") && isGrounded())
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
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1, 0.03f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // Apply water physics properties
            jumpPower = waterJumpPower;
            rb.gravityScale = waterGravityScale;
            fallMultiplier = fallMultiplier * waterGravityScale;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // Revert to original physics properties
            jumpPower = originalJumpPower;
            rb.gravityScale = originalGravityScale;
            fallMultiplier = originalfallMultiplier;
        }
    }
}
