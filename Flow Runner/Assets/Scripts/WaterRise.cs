using UnityEngine;

public class WaterRise : MonoBehaviour
{
    public float riseSpeed = 0.1f; // Speed at which the water rises

    // Update is called once per frame
    void Update()
    {
        if (!PauseManager.isPaused)
        {
            // Move the water object upwards at the specified rise speed
            transform.position += new Vector3(0, riseSpeed * Time.deltaTime, 0);
        }
    }
}
