using UnityEngine;
using UnityEngine.UI;

public class AirMeter : MonoBehaviour
{
    public Slider airSlider; // Reference to the UI slider
    public float maxAir = 100f; // Maximum air
    public float airConsumptionRate = 10f; // Rate at which air is consumed per second
    public float currentAir; // Current air
    public PlayerMovement playerMovement; // Assign this in the Unity Inspector
    public GameObject uiSlider; // Assign the GameObject containing the UI Slider in the Unity Inspector

    public void Start()
    {
        if (airSlider == null)
        {
            Debug.LogError("Air Slider is not assigned in the inspector.");
            return;
        }

        currentAir = maxAir;

        // Set up the slider only if it's assigned
        airSlider.maxValue = maxAir;
        airSlider.value = currentAir;
    }

    public void Update()
    {
        if (playerMovement == null)
        {
            Debug.LogError("Player Movement is not assigned in the inspector.");
            return;
        }

        // Consume air when the player is underwater
        if (playerMovement.IsInWater)
        {
            if (0 <= currentAir)
            {
                currentAir -= airConsumptionRate * Time.deltaTime;
                airSlider.value = currentAir;
            }
            uiSlider.SetActive(true); // Show the air meter

            if (currentAir <= 0)
            {
                // Handle drowning
                Debug.Log("Player has drowned!");
            }
        }
        else
        {

            // Replenish air when the player is not underwater
            if (currentAir < maxAir)
            {
                currentAir += airConsumptionRate * Time.deltaTime;
                airSlider.value = currentAir;
            }
            if (currentAir >= maxAir) 
            {
                uiSlider.SetActive(false); // Hide the air meter
            }
            
        }
    }

}
