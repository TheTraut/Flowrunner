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
        SettingsManager.Instance.Load();
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
        DestroyInactiveModals<GuideModalWindow>();
        DestroyInactiveModals<HighscoresModalWindow>();
        DestroyInactiveModals<SettingsModalWindow>();
    }

    /// <summary>
    /// Loads the guide scene.
    /// </summary>
    public void SeeGuide()
    {
        GuideModalWindow.Create()
            .SetHeader("Guide") // Set the header text
            .Guide() // Init
            .Show(); // Show the modal window
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
            .SetSettings(
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

    /// <summary>
    /// Destroys any modal windows that are not visible.
    /// </summary>
    private void DestroyInactiveModals<T>() where T : ModalWindow<T>
    {
        // Find all active modal windows
        T[] activeModals = FindObjectsOfType<T>();

        foreach (T modal in activeModals)
        {
            // Check if the modal window is not visible and its close animation is not playing
            if (!modal.Visible && IsClosingAnimationPlaying(modal))
            {
                Destroy(modal.gameObject);
            }
        }
    }

    private bool IsClosingAnimationPlaying<T>(T modal) where T : ModalWindow<T>
    {
        // Check if the modal's animation state is the closing animation
        return modal.animator.GetCurrentAnimatorStateInfo(0).IsName("Close");
    }
}