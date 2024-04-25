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