
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    public float currentScore = 0f;
    private string textScore;
    private Text score;

    /// <summary>
    /// Controls the display and calculation of the score.
    /// </summary>
    void Start()
    {
        score = GetComponent<Text>();
    }

    /// <summary>
    /// Rounds and converts the current score into a string for display.
    /// </summary>
    /// <param name="currentScore">The current score.</param>
    /// <returns>A string representation of the current score.</returns>
    public string TextScore( float currentScore) // rounds and converts score into a string to be displayed 
    {
        return Mathf.RoundToInt(currentScore).ToString();
    }

    /// <summary>
    /// Updates the current score and updates the text display.
    /// </summary>
    void Update()
    {
        if (!PauseManager.isPaused)
        {
            currentScore += (Time.deltaTime * 100); // Score calculation 
            textScore = TextScore(currentScore);
            score.text = textScore;
        }
    }

}
