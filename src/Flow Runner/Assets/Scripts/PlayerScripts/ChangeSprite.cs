using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color underwaterColor = Color.blue;
    private Quaternion originalRotation;
    private Quaternion underwaterRotation = Quaternion.Euler(0, 0, 270);
    public PlayerMovement playerMovement; // Assign this in the Unity Inspector
    public BuoyancyEffector2D waterBuoyancyEffector;
    public GameObject waterGameObject; // Assign the water GameObject in the inspector
    public float waterSurfaceLevel;

    /// <summary>
    /// Manages sprite changes and rotations based on the player's position relative to water.
    /// </summary>
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            originalRotation = transform.rotation;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
        }

        // Get the BuoyancyEffector2D component from the water GameObject
        if (waterGameObject != null)
        {
            waterBuoyancyEffector = waterGameObject.GetComponent<BuoyancyEffector2D>();
        }
    }

    /// <summary>
    /// Updates the sprite and rotation based on the player's position relative to water.
    /// </summary>
    public void Update()
    {

        if (waterBuoyancyEffector != null && !PauseManager.IsPaused)
        {
            waterSurfaceLevel = waterGameObject.transform.position.y + waterGameObject.transform.localScale.y / 2;
            if (transform.position.y < waterSurfaceLevel)
            {
                ChangeToUnderwaterSprite();
                playerMovement.isUnderWater = true;
            }
            else
            {
                ResetToOriginalSprite();
                playerMovement.isUnderWater = false;
            }
        }
    }

    /// <summary>
    /// Changes the sprite and rotation to represent being underwater.
    /// </summary>
    private void ChangeToUnderwaterSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = underwaterColor;
            transform.rotation = underwaterRotation;
        }
    }

    /// <summary>
    /// Resets the sprite and rotation to the original state.
    /// </summary>
    private void ResetToOriginalSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
            transform.rotation = originalRotation;
        }
    }

    /// <summary>
    /// Handles actions when entering a trigger collider.
    /// </summary>
    /// <param name="other">The Collider2D object the GameObject has collided with.</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water") && !PauseManager.IsPaused)
        {
            // Set color to underwater and rotate
            if (spriteRenderer != null)
            {
                spriteRenderer.color = underwaterColor;
            }
            transform.rotation = underwaterRotation;
        }
    }

    /// <summary>
    /// Handles actions when exiting a trigger collider.
    /// </summary>
    /// <param name="other">The Collider2D object the GameObject has exited.</param>
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water") && !PauseManager.IsPaused)
        {
            // Reset color and rotation to original
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
            transform.rotation = originalRotation;
        }
    }
}