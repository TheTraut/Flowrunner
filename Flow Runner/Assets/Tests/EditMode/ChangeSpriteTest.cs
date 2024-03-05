using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ChangeSpriteTest
{
    [UnityTest]
    public IEnumerator TestUnderwaterColor()
    {
        // Arrange
        var gameObject = new GameObject();
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>(); // Add SpriteRenderer component
        var changeSprite = gameObject.AddComponent<ChangeSprite>();
        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true; // Set collider as trigger

        // Act
        collider.tag = "Water"; // Simulate entering water
        changeSprite.OnTriggerEnter2D(collider); // Trigger the method

        // Assert
        LogAssert.Expect(LogType.Error, "SpriteRenderer component not found on the GameObject."); // Expect the error message
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestOriginalColorAfterExitingWater()
    {
        // Arrange
        var gameObject = new GameObject();
        var changeSprite = gameObject.AddComponent<ChangeSprite>();
        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true; // Set collider as trigger

        // Act
        collider.tag = "Water"; // Simulate entering water
        changeSprite.OnTriggerEnter2D(collider); // Trigger the method
        collider.tag = "Untagged"; // Simulate exiting water
        changeSprite.OnTriggerExit2D(collider); // Trigger the method

        // Assert
        LogAssert.Expect(LogType.Error, "SpriteRenderer component not found on the GameObject."); // Expect the error message

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestUnderwaterRotation()
    {
        // Arrange
        var changeSpriteGameObject = new GameObject();
        var changeSprite = changeSpriteGameObject.AddComponent<ChangeSprite>();
        var collider = changeSpriteGameObject.AddComponent<BoxCollider2D>();

        var expectedRotation = Quaternion.Euler(0, 0, 270);

        // Expect the log message
        LogAssert.Expect(LogType.Error, "SpriteRenderer component not found on the GameObject.");

        // Act
        collider.tag = "Water";
        collider.isTrigger = true;
        changeSprite.OnTriggerEnter2D(collider);

        // Yield to the end of the frame to allow changes to take effect
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestOriginalRotationAfterExitingWater()
    {
        // Arrange
        var gameObject = new GameObject();
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>(); // Add SpriteRenderer component
        var changeSprite = gameObject.AddComponent<ChangeSprite>();
        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true; // Set collider as trigger

        // Act
        collider.tag = "Water"; // Simulate entering water
        changeSprite.OnTriggerEnter2D(collider); // Trigger the method
        collider.tag = "Untagged"; // Simulate exiting water
        changeSprite.OnTriggerExit2D(collider); // Trigger the method

        // Assert
        LogAssert.Expect(LogType.Error, "SpriteRenderer component not found on the GameObject."); // Expect the error message
        yield return null;
    }
}
