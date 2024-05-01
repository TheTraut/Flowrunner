using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        public Button[] deleteButtons;
        public TMP_Text noScoresText;
        public TMP_Text[] headerTexts;
    }

    #pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] private HighscoreTexts highscoreTexts;
    [SerializeField] private GameObject[] rowLines;
    [SerializeField] private GameObject[] otherLines;
    [SerializeField] private Button undoButton;
    [SerializeField] private Button redoButton;
    #pragma warning restore IDE0044 // Add readonly modifier

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
    /// Loads settings when the object is awake.
    /// </summary>
    private void Awake()
    {
        HighscoresManager.Instance.LoadHighscores();
        UpdateUndoRedoButtonVisibility();
        undoButton.onClick.AddListener(() =>
        {
            History.Instance.Undo();
            UpdateUndoRedoButtonVisibility();
            PopulateHighscores();
        });
        redoButton.onClick.AddListener(() =>
        {
            History.Instance.Redo();
            UpdateUndoRedoButtonVisibility();
            PopulateHighscores();
        });
    }

    public override HighscoresModalWindow Close()
    {
        Instance = null;
        base.Close();
        return Instance;
    }

    /// <summary>
    /// Updates the visibility of the undo and redo buttons based on the command history state.
    /// </summary>
    private void UpdateUndoRedoButtonVisibility()
    {
        bool canUndo = History.Instance.CanUndo();
        bool canRedo = History.Instance.CanRedo();

        SetButtonVisibility(undoButton, canUndo);
        SetButtonVisibility(redoButton, canRedo);
    }

    private void SetButtonVisibility(Button button, bool isVisible)
    {
        CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = isVisible ? 1f : 0f;
            canvasGroup.blocksRaycasts = isVisible;
        }
    }

    /// <summary>
    /// Closes the highscores modal.
    /// </summary>
    public void CloseHighscores()
    {
        History.Instance.DeleteInstance();
        Close();
    }

    /// <summary>
    /// Populates the highscores in the UI.
    /// </summary>
    private void PopulateHighscores()
    {
        List<HighscoreEntry> highscores = HighscoresManager.Instance.GetHighscores();

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
            highscoreTexts.noScoresText.gameObject.SetActive(true);
            highscoreTexts.noScoresText.text = "No high scores";
            SetAllTextsEmpty();
            return;
        }
        else
        {
            foreach (TMP_Text headerText in highscoreTexts.headerTexts)
            {
                headerText.gameObject.SetActive(true);
            }
            foreach (GameObject line in otherLines)
            {
                line.SetActive(true);
            }
            highscoreTexts.noScoresText.gameObject.SetActive(false);
        }

        int maxEntries = Mathf.Min(highscores.Count, highscoreTexts.nameTexts.Length);
        for (int i = 0; i < highscoreTexts.nameTexts.Length; i++)
        {
            if (i < maxEntries)
            {
                // Display highscore entry
                highscoreTexts.nameTexts[i].text = highscores[i].playerName;
                highscoreTexts.scoreTexts[i].text = highscores[i].score.ToString();
                if (highscores[i].playerName == SettingsManager.Instance.PlayerName)
                {
                    // Remove existing onClick listeners (if any)
                    highscoreTexts.deleteButtons[i].onClick.RemoveAllListeners();
                    // Hide button
                    highscoreTexts.deleteButtons[i].GetComponent<CanvasGroup>().alpha = 1f;
                    highscoreTexts.deleteButtons[i].GetComponent<CanvasGroup>().blocksRaycasts = true;
                    int currentIndex = i; // Save current index for use in lambda
                    highscoreTexts.deleteButtons[i].onClick.AddListener(() => DeleteButton_OnClick(currentIndex));
                } else
                {
                    highscoreTexts.deleteButtons[i].GetComponent<CanvasGroup>().alpha = 0f;
                    highscoreTexts.deleteButtons[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
                //if (maxEntries >= highscoreTexts.nameTexts.Length && i < maxEntries - 1) // Check if it's the last iteration when the table is full
                //{
                //    rowLines[i].SetActive(true); // Show the row line for this row
                //}
                if (i < highscoreTexts.nameTexts.Length - 1) // Check if it's the last iteration
                {
                    rowLines[i].SetActive(true); // Show the row line for this row
                }
            }
            else
            {
                // Hide remaining UI elements
                highscoreTexts.nameTexts[i].text = "";
                highscoreTexts.scoreTexts[i].text = "";
                highscoreTexts.deleteButtons[i].GetComponent<CanvasGroup>().alpha = 0f;
                highscoreTexts.deleteButtons[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
                if (i < highscoreTexts.nameTexts.Length - 1) // Check if it's the last iteration
                {
                    rowLines[i].SetActive(false); // Hide the row line for this row
                }
            }
        }
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

        foreach (var deleteButton in highscoreTexts.deleteButtons)
        {
            deleteButton.GetComponent<CanvasGroup>().alpha = 0f;
            deleteButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        foreach (var line in rowLines)
        {
            line.SetActive(false); // Hide all row lines when there are no scores
        }
    }

    /// <summary>
    /// Handles the onClick event for delete buttons in the highscores modal window.
    /// Removes the highscore entry corresponding to the button's index.
    /// </summary>
    /// <param name="index">The index of the highscore entry to delete.</param>
    private void DeleteButton_OnClick(int index)
    {
        HighscoreEntry entry = HighscoresManager.Instance.GetHighscores()[index];
        Command deleteCommand = new DeleteHighscoreCommand(entry);
        History.Instance.Do(deleteCommand); // Store the deletion command in the history
        PopulateHighscores(); // Update the UI after deletion
        UpdateUndoRedoButtonVisibility();
    }
}