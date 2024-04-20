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

    public void Update()
    {

        if (waterBuoyancyEffector != null && !PauseManager.isPaused)
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

    private void ChangeToUnderwaterSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = underwaterColor;
            transform.rotation = underwaterRotation;
        }
    }

    private void ResetToOriginalSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
            transform.rotation = originalRotation;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water") && !PauseManager.isPaused)
        {
            // Set color to underwater and rotate
            if (spriteRenderer != null)
            {
                spriteRenderer.color = underwaterColor;
            }
            transform.rotation = underwaterRotation;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water") && !PauseManager.isPaused)
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