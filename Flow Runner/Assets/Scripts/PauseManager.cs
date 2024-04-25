using UnityEngine;

/// <summary>
/// Manages the pause state of the game.
/// </summary>
public static class PauseManager
{
    private static bool isGamePaused = false;

    public static bool isPaused
    {
        get { return isGamePaused; }
    }

    /// <summary>
    /// Pauses the game by setting the time scale to zero.
    /// </summary>
    public static void Pause()
    {
        Time.timeScale = 0f; // Set the time scale to zero to pause the game
        isGamePaused = true;
    }

    /// <summary>
    /// Resumes the game by setting the time scale back to one.
    /// </summary>
    public static void Resume()
    {
        Time.timeScale = 1f; // Set the time scale back to 1 to resume the game
        isGamePaused = false;
    }
}