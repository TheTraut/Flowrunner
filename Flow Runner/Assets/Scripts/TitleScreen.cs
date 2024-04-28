using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages interactions and UI elements on the title screen.
/// </summary>
public class TitleScreen : MonoBehaviour
{
    public Text playerNameLabel;

    /// <summary>
    /// Initializes the title screen by loading player settings.
    /// </summary>
    private void Awake()
    {
        SettingsManager.Instance.LoadSettings();
    }

    /// <summary>
    /// Loads the main game scene.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    /// <summary>
    /// Updates UI elements with player name.
    /// </summary>
    void Update()
    {
        SetName();
    }

    /// <summary>
    /// Loads the guide scene.
    /// </summary>
    public void SeeGuide()
    {
        SceneManager.LoadSceneAsync("Guide1");
    }

    /// <summary>
    /// Displays a message indicating that highscores feature is coming soon.
    /// </summary>
    public void SeeHighScores()
    {
        //ToastModalWindow.Create(ignorable: true)
        //        .SetHeader("Coming Soon")
        //        .SetBody("Highscores are being added, check back soon!")
        //        .SetDelay(3f) // Set it to 0 to make popup persistent
        //                      //.SetIcon(sprite) // Also you can set icon
        //        .Show();

        // Create and display the HighscoresModalWindow
        HighscoresModalWindow.Create()
            .SetHeader("High Scores") // Set the header text
            .Highscores() // Populate the high scores
            .Show(); // Show the modal window
    }

    /// <summary>
    /// Opens the settings modal window to allow players to adjust settings.
    /// </summary>
    public void SeeSettings()
    {
        SettingsModalWindow.Create()
            .SetHeader("Settings")
            .SetSettings((newName, newVolume, upKeys, downKeys, shieldKeys) =>
            {
                List<KeyCode> upKey = new List<KeyCode>(upKeys);
                List<KeyCode> downKey = new List<KeyCode>(downKeys);
                List<KeyCode> shieldKey = new List<KeyCode>(shieldKeys);
                SettingsManager.Instance.UpdateSettings(newName, newVolume, upKey, downKey, shieldKey);
            },
            SettingsManager.Instance.PlayerName,
            SettingsManager.Instance.Volume / 100f,
            "Enter your name",
            SettingsManager.Instance.UpShortcutKeys,
            SettingsManager.Instance.DownShortcutKeys,
            SettingsManager.Instance.ShieldShortcutKeys)
            .Show();
    }

    /// <summary>
    /// Sets the player name on the UI.
    /// </summary>
    private void SetName()
    {
        string playerName = SettingsManager.Instance.PlayerName;
        playerNameLabel.text = playerName;
    }
}