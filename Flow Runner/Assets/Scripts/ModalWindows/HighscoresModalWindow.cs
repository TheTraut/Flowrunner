using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the highscores modal window.
/// </summary>
public class HighscoresModalWindow : ModalWindow<HighscoresModalWindow>
{
    [Serializable]
    public class HighscoreTexts
    {
        public TMP_Text[] nameTexts;
        public TMP_Text[] scoreTexts;
        public TMP_Text noScoresText;
        public TMP_Text[] headerTexts;
    }

    [SerializeField] private HighscoreTexts highscoreTexts;
    [SerializeField] private GameObject[] rowLines;
    [SerializeField] private GameObject[] otherLines;

    /// <summary>
    /// Opens the highscores modal.
    /// </summary>
    /// <returns>The current instance of the highscores modal window.</returns>
    public HighscoresModalWindow Highscores()
    {
        PopulateHighscores();
        return this;
    }

    /// <summary>
    /// Populates the highscores in the UI.
    /// </summary>
    private void PopulateHighscores()
    {
        List<(string, int)> highscores = HighscoresManager.Instance.GetHighscores();

        if (highscores.Count == 0)
        {
            foreach (TMP_Text headerText in highscoreTexts.headerTexts)
            {
                headerText.gameObject.SetActive(false);
            }
            foreach (GameObject line in otherLines)
            {
                line.SetActive(false);
            }
            highscoreTexts.noScoresText.text = "No high scores";
            SetAllTextsEmpty();
            return;
        }

        highscoreTexts.noScoresText.text = "";

        int count = Mathf.Min(highscores.Count, highscoreTexts.nameTexts.Length);

        for (int i = 0; i < count; i++)
        {
            highscoreTexts.nameTexts[i].text = highscores[i].Item1;
            highscoreTexts.scoreTexts[i].text = highscores[i].Item2.ToString();
            rowLines[i].SetActive(true); // Show the row line for this row
        }

        SetRemainingTextsEmpty(count);
    }

    /// <summary>
    /// Sets all text elements to empty.
    /// </summary>
    private void SetAllTextsEmpty()
    {
        foreach (var text in highscoreTexts.nameTexts)
        {
            text.text = "";
        }

        foreach (var text in highscoreTexts.scoreTexts)
        {
            text.text = "";
        }

        foreach (var line in rowLines)
        {
            line.SetActive(false); // Hide all row lines when there are no scores
        }
    }

    /// <summary>
    /// Sets remaining text elements to empty based on the count.
    /// </summary>
    /// <param name="count">Number of high scores to display.</param>
    private void SetRemainingTextsEmpty(int count)
    {
        for (int i = count; i < highscoreTexts.nameTexts.Length; i++)
        {
            highscoreTexts.nameTexts[i].text = "";
            highscoreTexts.scoreTexts[i].text = "";
            rowLines[i].SetActive(false); // Hide the row line for this row
        }
    }

    /// <summary>
    /// Loads settings when the object is awake.
    /// </summary>
    private void Awake()
    {
        HighscoresManager.Instance.LoadHighscores();
    }

    /// <summary>
    /// Closes the highscores modal.
    /// </summary>
    public void CloseHighscores()
    {
        Close();
    }
}