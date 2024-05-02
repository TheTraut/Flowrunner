using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the pause state of the game.
/// </summary>
public static class PauseManager
{
    private static bool isGamePaused = false;
    private static readonly List<Rigidbody2D> affectedNonStaticRigidbodies = new(); // Non-static rigidbodies affected by pause
    private static readonly List<Vector2> originalVelocities = new(); // Store original velocities of rigidbodies
    private static readonly List<float> originalGravityScales = new(); // Store original gravity scales of rigidbodies
    private static readonly List<bool> originalSimulatedStates = new(); // Store original simulated state of rigidbodies

    public static bool IsPaused
    {
        get { return isGamePaused; }
    }

    /// <summary>
    /// Pauses the game by setting the time scale to zero and freezing velocities.
    /// </summary>
    public static void Pause()
    {
        // Find all affected rigidbodies
        Rigidbody2D[] allRigidbodies = GameObject.FindObjectsOfType<Rigidbody2D>();

        // Filter out non-static rigidbodies
        foreach (Rigidbody2D rb in allRigidbodies)
        {
            if (!rb.isKinematic && rb.bodyType != RigidbodyType2D.Static) // Check if it's neither kinematic nor static
            {
                affectedNonStaticRigidbodies.Add(rb);
            }
        }

        // Store original velocities, gravity scales, and simulated state of non-static rigidbodies
        // Followed by freezing their velocities
        foreach (Rigidbody2D rb in affectedNonStaticRigidbodies)
        {
            originalVelocities.Add(rb.velocity);
            originalGravityScales.Add(rb.gravityScale);
            originalSimulatedStates.Add(rb.simulated);

            rb.velocity = Vector2.zero; // Freeze velocities on x and y axes
            rb.gravityScale = 0f; // Freeze gravity
            rb.simulated = false; // Disable simulation
        }

        // Pause the game
        //Time.timeScale = 0f;
        isGamePaused = true;
    }

    /// <summary>
    /// Resumes the game by setting the time scale back to one and restoring velocities, gravity scales, and simulated state.
    /// </summary>
    public static void Resume()
    {
        // Restore velocities, gravity scales, and simulated state of non-static rigidbodies
        for (int i = 0; i < affectedNonStaticRigidbodies.Count; i++)
        {
            affectedNonStaticRigidbodies[i].velocity = originalVelocities[i];
            affectedNonStaticRigidbodies[i].gravityScale = originalGravityScales[i];
            affectedNonStaticRigidbodies[i].simulated = originalSimulatedStates[i];
        }

        // Resume the game
        //Time.timeScale = 1f;
        isGamePaused = false;

        // Clear stored data
        affectedNonStaticRigidbodies.Clear();
        originalVelocities.Clear();
        originalGravityScales.Clear();
        originalSimulatedStates.Clear();
    }
}