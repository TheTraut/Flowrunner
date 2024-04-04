using UnityEngine;
using System.IO;

public class SettingsManager : MonoBehaviour
{
    public string playerName = "Player";
    public int soundVolume = 50;
    private string settingsFilePath;

    void Awake()
    {
        settingsFilePath = Application.persistentDataPath + "/settings.json";
        if (File.Exists(settingsFilePath))
        {
            LoadSettings();
        } else
        {
            SaveSettings();
        }
    }

    public void SaveSettings()
    {
        SettingsData data = new SettingsData(playerName, soundVolume);
        string jsonData = JsonUtility.ToJson(data);

        File.WriteAllText(settingsFilePath, jsonData);
    }

    public void LoadSettings()
    {
        string jsonData = File.ReadAllText(settingsFilePath);
        SettingsData data = JsonUtility.FromJson<SettingsData>(jsonData);

        playerName = data.playerName;
        soundVolume = data.soundVolume;
    }

    public void UpdateSettings(string name, int volume)
    {
        playerName = name;
        soundVolume = Mathf.Clamp(volume, 1, 100);

        SaveSettings();
    }
}

[System.Serializable]
public class SettingsData
{
    public string playerName;
    public int soundVolume;

    public SettingsData(string name, int volume)
    {
        this.playerName = name;
        this.soundVolume = volume;
    }
}