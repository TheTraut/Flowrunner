using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class AirMeterTest
{
    GameObject playerGameObject;
    AirMeter airMeter;
    Slider slider;
    /// <summary>
    /// Creating and assigned objects
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // Create and setup all necessary GameObjects and components for the tests
        playerGameObject = new GameObject();
        airMeter = playerGameObject.AddComponent<AirMeter>();
        slider = new GameObject().AddComponent<Slider>();
        airMeter.airSlider = slider;
        airMeter.maxAir = 100f;
        airMeter.currentAir = 100f;
        airMeter.airConsumptionRate = 10f;
        airMeter.uiSlider = new GameObject();
        airMeter.uiSlider.SetActive(false);
    }

    /// <summary>
    /// Tests for air slider being null
    /// </summary>
    [Test]
    public void Start_AirSliderIsNull_LogsError()
    {
        airMeter.airSlider = null; // Not assigning the airSlider to simulate the error condition

        LogAssert.Expect(LogType.Error, "Air Slider is not assigned in the inspector.");
        airMeter.Start();
    }

    /// <summary>
    /// Tests if the air meter is assigned correctly  
    /// </summary>
    [Test]
    public void Start_AirSliderIsAssigned_SetsInitialValuesCorrectly()
    {
        airMeter.airSlider = slider; // Assigning the slider
        airMeter.maxAir = 100f; // Setting max air

        airMeter.Start();

        Assert.AreEqual(100f, airMeter.currentAir, "Current air is not set to max air.");
        Assert.AreEqual(100f, slider.maxValue, "Slider max value is not set correctly.");
        Assert.AreEqual(100f, slider.value, "Slider value is not set correctly.");
    }

    /// <summary>
    /// Tests if the air is consumed at the correct rate when underwater.
    /// </summary>
    [UnityTest]
    public IEnumerator AirConsumesCorrectlyUnderwater()
    {
        airMeter.playerMovement = playerGameObject.AddComponent<PlayerMovement>();
        airMeter.playerMovement.isUnderWater = true;  // Mock player being underwater
        float initialAir = airMeter.currentAir;

        // Simulate a few seconds to see if air is consumed
        for (float time = 0; time < 1f; time += Time.deltaTime)
        {
            airMeter.Update();
            yield return null;
        }

        Assert.Less(airMeter.currentAir, initialAir, "Air should decrease when underwater.");
    }

    /// <summary>
    /// Tests if the air meter is replenished correctly when not underwater.
    /// </summary>
    [UnityTest]
    public IEnumerator AirReplenishesCorrectlyNotUnderwater()
    {
        airMeter.playerMovement = playerGameObject.AddComponent<PlayerMovement>();
        airMeter.playerMovement.isUnderWater = false;  // Mock player not being underwater
        airMeter.currentAir = 50f;  // Set a depleted air level
        float initialAir = airMeter.currentAir;

        // Simulate a few seconds to see if air is replenished
        for (float time = 0; time < 1f; time += Time.deltaTime)
        {
            airMeter.Update();
            yield return null;
        }

        Assert.Greater(airMeter.currentAir, initialAir, "Air should increase when not underwater.");
    }
    /// <summary>
    /// Tests if the air meter was not assigned
    /// </summary>
    [Test]
    public void Start_WithNoSliderAssigned_LogsError()
    {
        var airMeter = new GameObject().AddComponent<AirMeter>();
        airMeter.Start();
        LogAssert.Expect(LogType.Error, "Air Slider is not assigned in the inspector.");
    }

    /// <summary>
    /// Tests if the player movment is not assigned
    /// </summary>
    [Test]
    public void Update_WithNoPlayerMovementAssigned_LogsError()
    {
        var gameObject = new GameObject();
        var airMeter = gameObject.AddComponent<AirMeter>();
        airMeter.playerMovement = null;

        airMeter.Update();

        LogAssert.Expect(LogType.Error, "Player Movement is not assigned in the inspector.");
    }
    /// <summary>
    /// Tests if the air meter hides when replenished 
    /// </summary>
    [UnityTest]
    public IEnumerator Update_AirFullyReplenished_HidesAirMeter()
    {
        // Arrange
        var gameObject = new GameObject();
        var airMeter = gameObject.AddComponent<AirMeter>();
        airMeter.playerMovement = gameObject.AddComponent<PlayerMovement>();  // Ensure PlayerMovement is assigned
        airMeter.playerMovement.isUnderWater = false;  // Not underwater
        var slider = new GameObject().AddComponent<Slider>();
        airMeter.airSlider = slider;
        airMeter.uiSlider = new GameObject();  // UI Slider to be hidden
        airMeter.uiSlider.SetActive(true);  // Start visible

        airMeter.maxAir = 100f;
        airMeter.currentAir = 100f;  // Already fully replenished

        // Act
        airMeter.Update();
        yield return null;

        // Assert
        Assert.IsFalse(airMeter.uiSlider.activeSelf, "UI Slider should be hidden when air is fully replenished.");
    }
    /// <summary>
    /// Tests if the title screen loads when out of air
    /// </summary>
    [UnityTest]
    public IEnumerator Update_WithNoAir_LoadsTitleScreen()
    {
        // Arrange
        var gameObject = new GameObject("Player");
        var airMeter = gameObject.AddComponent<AirMeter>();
        airMeter.playerMovement = gameObject.AddComponent<PlayerMovement>();
        airMeter.airSlider = new GameObject().AddComponent<Slider>();
        airMeter.playerMovement.isUnderWater = true;
        airMeter.currentAir = 0;

        // Act
        airMeter.Update();

        // Use a small delay to simulate time for the scene to begin loading
        yield return null;

        // Assert
        // This assumes that the scene "Title Screen" is in the build settings and is loaded
        Assert.IsTrue(SceneManager.GetActiveScene().name == "Title Screen",
            "Expected 'Title Screen' but found '" + SceneManager.GetActiveScene().name + "'.");
    }

}
