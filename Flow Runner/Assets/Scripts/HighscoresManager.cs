using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
                    GameObject obj = new()
                    {
                        name = "HighscoresManager"
                    };
                    instance = obj.AddComponent<HighscoresManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    private List<HighscoreEntry> highscores;
    private readonly int MAX_HIGHSCORES = 10;
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
    public void AddHighscore(string playerName, int score, int? id = null)
    {
        if (id.HasValue)
        {
            highscores.Add(new HighscoreEntry(id.Value, playerName, score));
        }
        else
        {
            highscores.Add(new HighscoreEntry(GetNextAvailableId(), playerName, score));
        }

        highscores.Sort((a, b) => b.score.CompareTo(a.score)); // Sort in descending order based on score

        // Ensure only top MAX_HIGHSCORES scores are saved
        if (highscores.Count > MAX_HIGHSCORES)
        {
            highscores.RemoveAt(MAX_HIGHSCORES);
        }

        SaveHighscores();
    }

    /// <summary>
    /// Retrieves the next available ID for a new highscore entry.
    /// If there are existing entries, it returns the maximum ID plus one; otherwise, it returns 0.
    /// </summary>
    /// <returns>The next available ID.</returns>
    private int GetNextAvailableId()
    {
        // Find the maximum ID in the list and return the next available ID
        if (highscores.Count > 0)
        {
            return highscores.Max(entry => entry.id) + 1;
        }
        else
        {
            return 0; // Start from 0 if there are no entries
        }
    }

    /// <summary>
    /// Removes the highscore entry at the specified index from the highscores list.
    /// </summary>
    /// <param name="index">The index of the highscore entry to remove.</param>
    public void RemoveHighscore(int id)
    {
        int indexToRemove = highscores.FindIndex(entry => entry.id == id);
        if (indexToRemove != -1)
        {
            highscores.RemoveAt(indexToRemove);
            SaveHighscores(); // Save the updated highscores list
        }
        else
        {
            Debug.LogWarning("Highscore entry with ID " + id + " not found.");
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
    public int id; // Unique identifier for each entry
    public string playerName;
    public int score;

    public HighscoreEntry(int id, string name, int score)
    {
        this.id = id;
        playerName = name;
        this.score = score;
    }
}