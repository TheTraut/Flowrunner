using UnityEngine;

public class WaterRise : MonoBehaviour
{
    public float riseSpeed = 0.1f; // Speed at which the water rises

    /// <summary>
    /// Controls the rising movement of water.
    /// </summary>
    void Update()
    {
        if (!PauseManager.isPaused)
        {
            // Move the water object upwards at the specified rise speed
            transform.position += new Vector3(0, riseSpeed * Time.deltaTime, 0);
        }
    }
}