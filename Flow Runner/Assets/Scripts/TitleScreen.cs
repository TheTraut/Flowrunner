using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleScreen : MonoBehaviour
{
    public Text playerNameLabel;

    private void Awake()
    {
        SettingsManager.Instance.LoadSettings();
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    void Update()
    {
        SetName();
    }

    public void SeeGuide()
    {
        SceneManager.LoadSceneAsync("Guide1");
    }

    public void SeeHighScores()
    {
        ToastModalWindow.Create(ignorable: true)
                .SetHeader("Coming Soon")
                .SetBody("Highscores are being added, check back soon!")
                .SetDelay(3f) // Set it to 0 to make popup persistent
                              //.SetIcon(sprite) // Also you can set icon
                .Show();
    }

    public void SeeSettings()
    {
        SettingsModalWindow.Create()
            .SetHeader("Settings")
            .SetSettings((newName, newVolume) =>
            {
                SettingsManager.Instance.UpdateSettings(newName, newVolume);
            }, SettingsManager.Instance.PlayerName, (SettingsManager.Instance.Volume / 100f), "Enter your name")
            .SetShouldPauseOnClose(false)
            .Show();
    }

    private void SetName()
    {
        string playerName = SettingsManager.Instance.PlayerName;
        playerNameLabel.text = playerName;
    }
}
