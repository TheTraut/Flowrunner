using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;

public class WaterRiseTest
{
    private GameObject waterObject;
    private WaterRise waterRise;
    private float initialY;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and add the WaterRise component
        waterObject = new GameObject("Water");
        waterRise = waterObject.AddComponent<WaterRise>();
        waterObject.transform.position = new Vector3(0, 0, 0); // Start at the origin
        waterRise.Start(); // Manually call Start to initialize originalY
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up after each test
        Object.DestroyImmediate(waterObject);
    }

    /// <summary>
    /// Tests that the original y-position is correctly recorded on Start.
    /// </summary>
    [Test]
    public void WaterRise_Start_InitializesOriginalYCorrectly()
    {
        Assert.AreEqual(0, waterRise.originalY, "The original Y position should be recorded correctly on Start.");
    }

    /// <summary>
    /// Tests the oscillation of the water's Y position to ensure it follows a sinusoidal pattern.
    /// </summary>
    [UnityTest]
    public IEnumerator Update_OscillatesWaterPositionCorrectly()
    {
        float startTime = Time.time;
        float duration = 5.0f; // Test over 5 seconds for a full cycle
        float previousY = initialY;
        float minObservedY = float.MaxValue;
        float maxObservedY = float.MinValue;

        while (Time.time - startTime < duration)
        {
            waterRise.Update(); // Simulate the update method being called each frame
            yield return null;

            float currentY = waterObject.transform.position.y;
            minObservedY = Mathf.Min(minObservedY, currentY);
            maxObservedY = Mathf.Max(maxObservedY, currentY);

            // Verify the water is oscillating around the original Y
            Assert.IsTrue(currentY >= initialY - waterRise.amplitude && currentY <= initialY + waterRise.amplitude,
                $"Y position {currentY} out of expected amplitude range [{initialY - waterRise.amplitude}, {initialY + waterRise.amplitude}].");

            // Ensure the Y position is oscillating (not static)
            Assert.AreNotEqual(previousY, currentY, "The Y position should change each frame to simulate oscillation.");
            previousY = currentY;
        }

        // Check that the observed amplitude approximates the expected amplitude
        Assert.AreEqual(waterRise.amplitude, maxObservedY - minObservedY, 0.1f,
            "Observed amplitude should approximate the expected amplitude within a small tolerance.");
    }
}
