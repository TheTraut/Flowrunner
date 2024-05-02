using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public float currentScore = 0f;
    [SerializeField] private Text scoreText;

    /// <summary>
    /// Invoked just before the game object is disabled or destroyed.
    /// </summary>
    private void OnDisable()
    {
        HighscoresManager.Instance.AddHighscore(SettingsManager.Instance.PlayerName, (int)currentScore);
    }

    /// <summary>
    /// Sets the score text GameObject to update the displayed score.
    /// </summary>
    /// <param name="scoreText">The Text component responsible for displaying the score.</param>
    public void SetScoreText(Text scoreText)
    {
        this.scoreText = scoreText;
        UpdateScoreText();
    }

    /// <summary>
    /// Updates the displayed score text.
    /// </summary>
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = Mathf.RoundToInt(currentScore).ToString();
        }
    }

    /// <summary>
    /// Updates the current score.
    /// </summary>
    void Update()
    {
        if (!PauseManager.IsPaused)
        {
            currentScore += (Time.deltaTime * 100); // Score calculation 
            UpdateScoreText();
        }
    }
}