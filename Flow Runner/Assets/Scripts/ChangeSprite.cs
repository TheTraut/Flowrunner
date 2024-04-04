using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor; // Store the original color
    public Color underwaterColor = Color.blue;
    private Quaternion originalRotation;
    private Quaternion underwaterRotation = Quaternion.Euler(0, 0, 270);

    private void Start()
    {
        // Get the sprite renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Store the original color
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
        }

        originalRotation = transform.rotation; // Save the original rotation
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
            else
            {
                Debug.LogError("SpriteRenderer component not found on the GameObject.");
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
            else
            {
                Debug.LogError("SpriteRenderer component not found on the GameObject.");
            }
            transform.rotation = originalRotation;
        }
    }
}