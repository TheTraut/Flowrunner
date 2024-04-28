using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class HighscoresManager : MonoBehaviour
{
    /// <summary>
    /// Provides a singleton instance of the HighscoresManager.
    /// If an instance does not exist, it creates a new one.
    /// </summary>
    private static HighscoresManager instance;
    public static HighscoresManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HighscoresManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "HighscoresManager";
                    instance = obj.AddComponent<HighscoresManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    private List<HighscoreEntry> highscores;

    private const string highscoresFileName = "highscores.json";
    private string highscoresFilePath;

    /// <summary>
    /// Initializes the highscores manager.
    /// Loads highscores from the persistent storage on Awake.
    /// </summary>
    private void Awake()
    {
        highscoresFilePath = Path.Combine(Application.persistentDataPath, highscoresFileName);
        LoadHighscores();
    }

    /// <summary>
    /// Retrieves the list of highscore entries.
    /// </summary>
    /// <returns>The list of highscore entries.</returns>
    public List<HighscoreEntry> GetHighscores()
    {
        return highscores;
    }

    /// <summary>
    /// Adds a new highscore entry with the specified player name and score.
    /// If the number of highscores exceeds the limit (6), only the top 6 scores are kept.
    /// </summary>
    /// <param name="playerName">The name of the player.</param>
    /// <param name="score">The score achieved by the player.</param>
    public void AddHighscore(string playerName, int score)
    {
        highscores.Add(new HighscoreEntry(playerName, score));
        highscores.Sort((a, b) => b.score.CompareTo(a.score)); // Sort in descending order based on score

        // Ensure only top 6 scores are saved
        if (highscores.Count > 6)
        {
            highscores.RemoveAt(6);
        }

        SaveHighscores();
    }

    /// <summary>
    /// Removes the highscore entry at the specified index from the highscores list.
    /// </summary>
    /// <param name="index">The index of the highscore entry to remove.</param>
    public void RemoveHighscore(int index)
    {
        if (index >= 0 && index < highscores.Count)
        {
            highscores.RemoveAt(index);
            SaveHighscores(); // Save the updated highscores list
        }
        else
        {
            Debug.LogWarning("Invalid index to remove highscore: " + index);
        }
    }

    /// <summary>
    /// Saves the current list of highscores to the persistent storage.
    /// </summary>
    private void SaveHighscores()
    {
        HighscoresData data = new()
        {
            highscores = highscores // Assign the current highscores list
        };
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(highscoresFilePath, jsonData);
    }

    /// <summary>
    /// Loads the highscores from the persistent storage.
    /// If the highscores file does not exist, creates a new one.
    /// </summary>
    public void LoadHighscores()
    {
        highscoresFilePath = Path.Combine(Application.persistentDataPath, highscoresFileName);
        if (File.Exists(highscoresFilePath))
        {
            string jsonData = File.ReadAllText(highscoresFilePath);
            HighscoresData data = JsonUtility.FromJson<HighscoresData>(jsonData);
            highscores = data.highscores;
        }
        else
        {
            Debug.LogWarning("Highscores file not found. Creating new highscores file.");
            HighscoresData data = new();
            highscores = data.highscores;
            SaveHighscores();
        }
    }
}

/// <summary>
/// Represents the highscores list data for serialization.
/// </summary>
[System.Serializable]
public class HighscoresData
{
    public List<HighscoreEntry> highscores;
}

/// <summary>
/// Represents an individual highscore entry for serialization.
/// </summary>
[System.Serializable]
public class HighscoreEntry
{
    public string playerName;
    public int score;

    public HighscoreEntry(string name, int score)
    {
        playerName = name;
        this.score = score;
    }
}