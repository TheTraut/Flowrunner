using UnityEngine;

public static class ScoreManager
{
    public static int CurrentScore { get; private set; }

    public static void ResetScore()
    {
        CurrentScore = 0;
    }

    public static void AddScore(int scoreToAdd)
    {
        CurrentScore += scoreToAdd;
    }

    public static void SetScore(int score)
    {
        CurrentScore = score;
    }
}
