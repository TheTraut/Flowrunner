using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages interactions and UI elements on the game screen.
/// </summary>
public class GameScreen : MonoBehaviour
{
    /// <summary>
    /// Initializes the game screen by loading the highscore instance.
    /// </summary>
    private void Awake()
    {
        HighscoresManager.Instance.LoadHighscores();
    }

    /// <summary>
    /// Invoked just before the game screen is disabled or destroyed.
    /// </summary>
    private void OnDisable()
    {
        AddPlayerScoreToHighscores();
    }

    /// <summary>
    /// Retrieves the player's score and adds it to the high scores if it's in the top 6.
    /// </summary>
    private void AddPlayerScoreToHighscores()
    {
        // Get the player's current score
        float playerScore = ScoreController.Instance.currentScore;
        Debug.Log(playerScore);

        // Get the player's name from SettingsManager
        string playerName = SettingsManager.Instance.PlayerName;
        Debug.Log(playerName);

        // Add the player's score to the highscores if it's in the top 6
        HighscoresManager.Instance.AddHighscore(playerName, (int)playerScore);
    }
}