using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the pause modal window.
/// </summary>
public class PauseModalWindow : ModalWindow<PauseModalWindow>
{
    public static new PauseModalWindow Instance { get; set; }

    /// <summary>
    /// Opens the pause menu.
    /// </summary>
    /// <returns>The current instance of the pause modal window.</returns>
    public PauseModalWindow PauseMenu()
    {
        return this;
    }

    /// <summary>
    /// Loads settings when the object is awake.
    /// </summary>
    private void Awake()
    {
        Instance = this;
        SettingsManager.Instance.Load();
    }

    protected override void Update()
    {
        // disables 'ese' to close modal
    }

    public override PauseModalWindow Close()
    {
        Instance = null;
        base.Close();
        return Instance;
    }

    /// <summary>
    /// Resumes the game and closes the pause menu.
    /// </summary>
    public void UI_ResumeButton()
    {
        ClosePause();
    }

    /// <summary>
    /// Closes the pause menu.
    /// </summary>
    public void ClosePause()
    {
        CameraController cameraController = FindObjectOfType<CameraController>();
        cameraController?.TogglePause();
        Close();
    }

    /// <summary>
    /// Opens the settings menu temporarily while the game is paused.
    /// </summary>
    public void UI_SettingsButton()
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
    /// Quits the game and returns to the title screen.
    /// </summary>
    public void UI_QuitButton()
    {
        SceneManager.LoadSceneAsync("Title Screen");
        PauseManager.Resume();
    }
}