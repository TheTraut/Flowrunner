using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleScreen : MonoBehaviour
{
    private void Awake()
    {
        SettingsManager.Instance.LoadSettings();
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void SeeGuide()
    {
        SceneManager.LoadSceneAsync("Guide1");
    }

    public void SeeHighScores()
    {
        SceneManager.LoadSceneAsync("High Scores");
    }

    public void SeeSettings()
    {
        SettingsModalWindow.Create()
            .SetHeader("Settings")
            .SetSettings((newName, newVolume) =>
            {
                SettingsManager.Instance.UpdateSettings(newName, int.Parse(newVolume));
            }, SettingsManager.Instance.PlayerName, SettingsManager.Instance.Volume.ToString(), "Enter your name", "Enter between 1-100")
            .SetShouldPauseOnClose(false)
            .Show();
    }
}
