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

    private List<(string, int)> highscores;

    private const string highscoresFileName = "highscores.json";
    private string highscoresFilePath;

    private void Awake()
    {
        highscoresFilePath = Path.Combine(Application.persistentDataPath, highscoresFileName);
        LoadHighscores();
    }

    public void AddHighscore(string playerName, int score)
    {
        highscores.Add((playerName, score));
        highscores.Sort((a, b) => b.Item2.CompareTo(a.Item2)); // Sort in descending order based on score

        // Ensure only top 6 scores are saved
        if (highscores.Count > 6)
        {
            highscores.RemoveAt(6);
        }

        SaveHighscores();
    }

    public List<(string, int)> GetHighscores()
    {
        return highscores;
    }

    private void SaveHighscores()
    {
        string jsonData = JsonUtility.ToJson(highscores);
        File.WriteAllText(highscoresFilePath, jsonData);
    }

    public void LoadHighscores()
    {
        highscoresFilePath = Path.Combine(Application.persistentDataPath, highscoresFileName);
        if (File.Exists(highscoresFilePath))
        {
            string jsonData = File.ReadAllText(highscoresFilePath);
            highscores = JsonUtility.FromJson<List<(string, int)>>(jsonData);
        }
        else
        {
            Debug.LogWarning("Highscores file not found. Creating new highscores file.");
            highscores = new List<(string, int)>();
            SaveHighscores();
        }
    }
}