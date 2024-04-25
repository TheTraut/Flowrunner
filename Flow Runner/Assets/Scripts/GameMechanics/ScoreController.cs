
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    public float currentScore = 0f;
    private string textScore;
    private Text score;
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
    }
    public string TextScore( float currentScore) // rounds and converts score into a string to be displayed 
    {
        return Mathf.RoundToInt(currentScore).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        currentScore += (Time.deltaTime * 100); // Score calculation 
        textScore = TextScore(currentScore);
        score.text = textScore;
    }

}
