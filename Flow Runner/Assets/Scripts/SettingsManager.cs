using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

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
                    GameObject obj = new()
                    {
                        name = "SettingsManager"
                    };
                    instance = obj.AddComponent<SettingsManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    private string playerName;
    private float volume;
    private List<KeyCode> upShortcutKeys;
    private List<KeyCode> downShortcutKeys;
    private List<KeyCode> shieldShortcutKeys;

    public string PlayerName { get { return playerName; } }
    public float Volume { get { return volume; } }
    public List<KeyCode> UpShortcutKeys { get { return upShortcutKeys; } }
    public List<KeyCode> DownShortcutKeys { get { return downShortcutKeys; } }
    public List<KeyCode> ShieldShortcutKeys { get { return shieldShortcutKeys; } }

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

    // Setter for playerName
    public void SetName(string newName)
    {
        playerName = newName;
    }

    // Setter for volume
    public void SetVolume(float newVolume)
    {
        volume = newVolume;
    }

    public void SetUpShortcut(List<KeyCode> shortcut)
    {
        UpShortcutKeys.Clear();
        UpShortcutKeys.AddRange(shortcut);
    }

    public void SetDownShortcut(List<KeyCode> shortcut)
    {
        DownShortcutKeys.Clear();
        DownShortcutKeys.AddRange(shortcut);
    }

    public void SetShieldShortcut(List<KeyCode> shortcut)
    {
        ShieldShortcutKeys.Clear();
        ShieldShortcutKeys.AddRange(shortcut);
    }

    /// <summary>
    /// Saves current player settings to a file.
    /// </summary>
    public void SaveSettings()
    {
        Debug.Log("file saved");
        SettingsData data = new(playerName, volume, upShortcutKeys, downShortcutKeys, shieldShortcutKeys);
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
            upShortcutKeys = data.upShortcutKeys;
            downShortcutKeys = data.downShortcutKeys;
            shieldShortcutKeys = data.shieldShortcutKeys;
        }
        else
        {
            Debug.LogWarning("Settings file not found. Creating new settings file.");

            // Set default values
            playerName = "Player";
            volume = 80f;
            upShortcutKeys = new List<KeyCode>() { KeyCode.W };
            downShortcutKeys = new List<KeyCode>() { KeyCode.S };
            shieldShortcutKeys = new List<KeyCode>() { KeyCode.Space };

            // Create and save new settings file
            SaveSettings();
        }
    }
}

public static class SettingsManagerExtensions
{
    // Helper method to check if all keys in the combination are pressed simultaneously
    public static bool AreKeyCombinationsPressed(List<KeyCode> keys)
    {
        foreach (KeyCode key in keys)
        {
            if (!Input.GetKey(key))
            {
                return false; // Return false if any key in the combination is not pressed
            }
        }
        return true; // Return true if all keys in the combination are pressed
    }

    public static List<KeyCode> StringToKeyCodeList(string input)
    {
        string[] keyStrings = input.Split('+');
        List<KeyCode> keyCodes = new();
        foreach (string keyString in keyStrings)
        {
            if (Enum.TryParse(keyString.Trim(), out KeyCode parsedKeyCode))
            {
                keyCodes.Add(parsedKeyCode);
            }
        }
        return keyCodes;
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
    public List<KeyCode> upShortcutKeys;
    public List<KeyCode> downShortcutKeys;
    public List<KeyCode> shieldShortcutKeys;

    public SettingsData(string name, float volume, List<KeyCode> upKeys, List<KeyCode> downKeys, List<KeyCode> changeShieldKeys)
    {
        playerName = name;
        soundVolume = volume;
        upShortcutKeys = upKeys;
        downShortcutKeys = downKeys;
        shieldShortcutKeys = changeShieldKeys;
    }

    // Default constructor
    public SettingsData()
    {
        playerName = "Player";
        soundVolume = 80f;
        upShortcutKeys = new List<KeyCode>() { KeyCode.W };
        downShortcutKeys = new List<KeyCode>() { KeyCode.S };
        shieldShortcutKeys = new List<KeyCode>() { KeyCode.Space };
    }
}