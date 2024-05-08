using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages interactions and UI elements on the title screen.
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private AudioClip selectSoundClip;
    [SerializeField] private Text gameOverScoreText;

    void Start()
    {
        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = ScoreManager.CurrentScore.ToString();
        }
    }

    /// <summary>
    /// Loads the main game scene.
    /// </summary>
    public void RestartGame()
    {
        SoundFXManager.instance.PlaySoundFXClip(selectSoundClip, transform);
        ScoreManager.ResetScore();
        SceneManager.LoadSceneAsync("Game");
    }

    /// <summary>
    /// Loads the main game scene.
    /// </summary>
    public void TitleScreen()
    {
        SoundFXManager.instance.PlaySoundFXClip(selectSoundClip, transform);
        ScoreManager.ResetScore();
        SceneManager.LoadSceneAsync("Title Screen");
    }
}