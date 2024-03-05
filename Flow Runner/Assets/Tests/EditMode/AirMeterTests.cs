using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AirMeterTests
{
    [UnityTest]
    public IEnumerator TestAirConsumption()
    {
        // Arrange
        var airMeterGameObject = new GameObject();
        var airMeter = airMeterGameObject.AddComponent<AirMeter>();

        // Create a mock slider GameObject
        var sliderGameObject = new GameObject();
        var slider = sliderGameObject.AddComponent<Slider>();
        airMeter.airSlider = slider;

        airMeter.maxAir = 100f;

        // Act
        airMeter.Start(); // Call the Start method manually
        yield return null; // Wait for one frame to allow for initialization

        // Assert
        Assert.AreEqual(airMeter.maxAir, airMeter.currentAir);
    }
}
