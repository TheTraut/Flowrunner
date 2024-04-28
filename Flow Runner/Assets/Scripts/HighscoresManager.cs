using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class HighscoresManager : MonoBehaviour
{
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

    private void Awake()
    {
        highscoresFilePath = Path.Combine(Application.persistentDataPath, highscoresFileName);
        LoadHighscores();
    }

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

    public List<HighscoreEntry> GetHighscores()
    {
        return highscores;
    }

    private void SaveHighscores()
    {
        HighscoresData data = new()
        {
            highscores = highscores // Assign the current highscores list
        };
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(highscoresFilePath, jsonData);
    }

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

[System.Serializable]
public class HighscoresData
{
    public List<HighscoreEntry> highscores;
}

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