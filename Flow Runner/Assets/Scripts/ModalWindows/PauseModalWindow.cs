﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// Manages the pause modal window.
/// </summary>
public class PauseModalWindow : ModalWindow<PauseModalWindow>
{
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
        SettingsManager.Instance.LoadSettings();
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
        if (cameraController != null)
        {
            cameraController.TogglePause();
        }
        Close();
    }

    /// <summary>
    /// Opens the settings menu temporarily while the game is paused.
    /// </summary>
    public void UI_SettingsButton()
    {
        StartCoroutine(TemporaryUnpauseAndOpenSettings());
    }

    /// <summary>
    /// Temporarily unpauses the game and opens the settings modal window.
    /// </summary>
    private IEnumerator TemporaryUnpauseAndOpenSettings()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.25f);

        // Show the settings modal window
        SettingsModalWindow.Create()
            .SetHeader("Settings")
            .SetSettings((newName, newVolume) =>
            {
                SettingsManager.Instance.UpdateSettings(newName, newVolume);
            }, SettingsManager.Instance.PlayerName, (SettingsManager.Instance.Volume / 100f), "Enter your name")
            .SetShouldPauseOnClose(true)
            .Show();

        yield return new WaitForSeconds(0.25f);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Quits the game and returns to the title screen.
    /// </summary>
    public void UI_QuitButton()
    {
        PauseManager.Resume();
        SceneManager.LoadSceneAsync("Title Screen");
    }

    /// <summary>
    /// Updates the modal window.
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }
}