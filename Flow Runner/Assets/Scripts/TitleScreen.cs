using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleScreen : MonoBehaviour
{
    private string settingsFilePath;
    private string guideFilePath;
    void Awake()
    {
        settingsFilePath = Application.persistentDataPath + "/settings.json";
        if (File.Exists(settingsFilePath))
        {
            LoadSettings();
        }
        else
        {
            Debug.LogWarning("Settings file not found!");
        }
    }

    string playerName;
    int volume;
    private void LoadSettings()
    {
        string jsonData = File.ReadAllText(settingsFilePath);
        SettingsData data = JsonUtility.FromJson<SettingsData>(jsonData);

        playerName = data.playerName;
        volume = data.soundVolume;
    }


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    private void LoadGuide()
    {
        string jsonData = File.ReadAllText(guideFilePath);
        GuideData data = JsonUtility.FromJson<GuideData>(jsonData);

        //line1 = data.line1;
        //line2 = data.line2;
        //line3 = data.line3;
        //line4 = data.line4;
    }

    public void SeeGuide()
    {
        GuideModalWindow.Create()
            .SetHeader("Guide")
            .SetBody("Hello World.")
            .Show();
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
                playerName = newName;
                volume = int.Parse(newVolume); // Convert the string volume input to an integer
                SaveUpdatedSettings();
            }, playerName, volume.ToString(), "Enter your name", "Enter between 1-100")
            .Show();
    }

    void SaveUpdatedSettings()
    {
        // Get a reference to the SettingsManager instance
        SettingsManager settingsManager = FindObjectOfType<SettingsManager>();

        // Call the UpdateSettings method of the SettingsManager to save the updated settings
        settingsManager.UpdateSettings(playerName, volume);
    }
}
