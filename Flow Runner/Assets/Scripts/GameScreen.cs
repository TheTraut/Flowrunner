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
}