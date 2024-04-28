using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    /// <summary>
    /// Gets the instance of the ScoreController, creating one if none exists.
    /// </summary>
    /// <returns>The instance of ScoreController.</returns>
    private static ScoreController instance;
    public static ScoreController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreController>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "ScoreController";
                    instance = obj.AddComponent<ScoreController>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    public float currentScore = 0f;
    [SerializeField] private Text scoreText;

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
        if (!PauseManager.isPaused)
        {
            currentScore += (Time.deltaTime * 100); // Score calculation 
            UpdateScoreText();
        }
    }
}