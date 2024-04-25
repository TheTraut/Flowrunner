using UnityEngine;
using System.IO;

public class SettingsManager : MonoBehaviour
{
    /// <summary>
    /// Gets the instance of the SettingsManager, creating one if none exists.
    /// </summary>
    /// <returns>The instance of SettingsManager.</returns>
    private static SettingsManager instance;
    public static SettingsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SettingsManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "SettingsManager";
                    instance = obj.AddComponent<SettingsManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    private string playerName;
    private float volume;

    public string PlayerName { get { return playerName; } }
    public float Volume { get { return volume; } }

    private const string settingsFileName = "settings.json";
    private string settingsFilePath;

    /// <summary>
    /// Initializes the SettingsManager instance.
    /// </summary>
    private void Awake()
    {
        settingsFilePath = Path.Combine(Application.persistentDataPath, settingsFileName);
        LoadSettings();
    }

    /// <summary>
    /// Updates player settings with new values.
    /// </summary>
    /// <param name="newName">The new player name.</param>
    /// <param name="newVolume">The new volume level.</param>
    public void UpdateSettings(string newName, float newVolume)
    {
        playerName = newName;
        volume = newVolume;
        SaveSettings();
    }

    /// <summary>
    /// Saves current player settings to a file.
    /// </summary>
    private void SaveSettings()
    {
        SettingsData data = new SettingsData(playerName, volume);

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(settingsFilePath, jsonData);
    }

    /// <summary>
    /// Loads player settings from a file, or creates new settings with default values if no file exists.
    /// </summary>
    public void LoadSettings()
    {
        settingsFilePath = Path.Combine(Application.persistentDataPath, settingsFileName);
        if (File.Exists(settingsFilePath))
        {
            string jsonData = File.ReadAllText(settingsFilePath);
            SettingsData data = JsonUtility.FromJson<SettingsData>(jsonData);

            playerName = data.playerName;
            volume = data.soundVolume;
        }
        else
        {
            Debug.LogWarning("Settings file not found. Creating new settings file.");

            // Set default values
            playerName = "Player";
            volume = 80f;

            // Create and save new settings file
            SaveSettings();
        }
    }
}

/// <summary>
/// Represents player settings data for serialization.
/// </summary>
[System.Serializable]
public class SettingsData
{
    public string playerName;
    public float soundVolume;

    public SettingsData(string name, float volume)
    {
        playerName = name;
        soundVolume = volume;
    }

    // Default constructor
    public SettingsData()
    {
        playerName = "Player";
        soundVolume = 80f;
    }
}