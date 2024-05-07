using UnityEngine;

public class WaterRise : MonoBehaviour
{
    public float amplitude = 4.0f; // The maximum distance the water moves up or down from its start position.
    public float frequency = 0.02f; // Speed of the oscillation.

    public float originalY; // Original y-position of the water.

    /// <summary>
    /// Initializes the original y-position of the water.
    /// </summary>
    public void Start()
    {
        originalY = transform.position.y;
    }

    /// <summary>
    /// Controls the oscillating movement of water.
    /// </summary>
    public void Update()
    {
        if (!PauseManager.IsPaused)
        {
            // Calculate the new y position using the sine of time.
            float newY = originalY + Mathf.Sin(Time.time * frequency) * amplitude;

            // Update the water's position with the new y value while keeping x and z the same.
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
