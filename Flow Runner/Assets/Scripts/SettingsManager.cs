using UnityEngine;
using System.IO;

public class SettingsManager : MonoBehaviour
{
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

    private void Awake()
    {
        settingsFilePath = Path.Combine(Application.persistentDataPath, settingsFileName);
        LoadSettings();
    }

    public void UpdateSettings(string newName, float newVolume)
    {
        playerName = newName;
        volume = newVolume;
        SaveSettings();
    }

    private void SaveSettings()
    {
        SettingsData data = new SettingsData(playerName, volume);

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(settingsFilePath, jsonData);
    }

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