using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    /// <summary>
    /// Invoked just before the game object is disabled or destroyed.
    /// </summary>
    private void OnDisable()
    {
        HighscoresManager.Instance.AddHighscore(SettingsManager.Instance.PlayerName, ScoreManager.CurrentScore);
    }

    /// <summary>
    /// Updates the displayed score text.
    /// </summary>
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = ScoreManager.CurrentScore.ToString();
        }
    }

    /// <summary>
    /// Updates the current score.
    /// </summary>
    void Update()
    {
        if (!PauseManager.IsPaused && SceneManager.GetActiveScene().name == "Game")
        {
            ScoreManager.AddScore(Mathf.RoundToInt(Time.deltaTime * 100));
            UpdateScoreText();
        }
    }
}