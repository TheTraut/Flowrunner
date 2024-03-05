using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; //
    public Color underwaterColor = Color.blue; // Color for underwater
    private Quaternion originalRotation; // To store the original rotation
    private Quaternion underwaterRotation = Quaternion.Euler(0, 0, 270); // Set to horizontal rotation


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalRotation = transform.rotation; // Save the original rotation
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            spriteRenderer.color = underwaterColor; // Set color to underwater
            transform.rotation = underwaterRotation; // Rotate to horizontal
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            spriteRenderer.color = Color.white; // Reset to original color
            transform.rotation = originalRotation; // Reset to original rotation

        }
    }
}
