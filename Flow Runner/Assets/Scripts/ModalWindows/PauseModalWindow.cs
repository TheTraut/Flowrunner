using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PauseModalWindow : ModalWindow<PauseModalWindow>
{

    public PauseModalWindow PauseMenu()
    {
        return this;
    }

    private void Awake()
    {
        SettingsManager.Instance.LoadSettings();
    }

    public void UI_ResumeButton()
    {
        ClosePause();
    }

    public void ClosePause()
    {
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            cameraController.TogglePause();
        }
        Close();
    }

    public void UI_SettingsButton()
    {
        StartCoroutine(TemporaryUnpauseAndOpenSettings());
    }

    private IEnumerator TemporaryUnpauseAndOpenSettings()
    {
        // Unpause the game for 0.3 seconds
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.3f);

        // Show the settings modal window
        SettingsModalWindow.Create()
            .SetHeader("Settings")
            .SetSettings((newName, newVolume) =>
            {
                SettingsManager.Instance.UpdateSettings(newName, int.Parse(newVolume));
            }, SettingsManager.Instance.PlayerName, SettingsManager.Instance.Volume.ToString(), "Enter your name", "Enter between 1-100")
            .SetShouldPauseOnClose(true)
            .Show();

        // Repause the game after 0.3 seconds
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0f;
    }

    public void UI_QuitButton()
    {
        PauseManager.Resume();
        SceneManager.LoadSceneAsync("Title Screen");
    }

    protected override void Update()
    {
        base.Update();
    }
}