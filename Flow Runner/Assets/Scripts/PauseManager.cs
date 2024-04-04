using UnityEngine;

public static class PauseManager
{
    private static bool isGamePaused = false;

    public static bool isPaused
    {
        get { return isGamePaused; }
    }

    public static void Pause()
    {
        Time.timeScale = 0f; // Set the time scale to zero to pause the game
        isGamePaused = true;
    }

    public static void Resume()
    {
        Time.timeScale = 1f; // Set the time scale back to 1 to resume the game
        isGamePaused = false;
    }
}