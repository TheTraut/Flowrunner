using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the settings modal window.
/// </summary>
public class SettingsModalWindow : ModalWindow<SettingsModalWindow>
{
    [SerializeField] private InputField nameField;
    [SerializeField] private Slider volumeSlider;

    private Action<string, float> onInputFieldsDone;
    private bool shouldPauseOnClose = true;

    /// <summary>
    /// Sets up the settings modal window with the specified settings.
    /// </summary>
    /// <param name="onDone">The action to be performed when settings are done.</param>
    /// <param name="initialValue">The initial value for the input field.</param>
    /// <param name="volumeInitialValue">The initial value for the volume slider.</param>
    /// <param name="placeholderValue">The placeholder value for the input field.</param>
    /// <returns>The current instance of the settings modal window.</returns>
    public SettingsModalWindow SetSettings(Action<string, float> onDone, string initialValue = "", float volumeInitialValue = 0.8f, string placeholderValue = "Type here")
    {
        nameField.text = initialValue;
        ((Text)nameField.placeholder).text = placeholderValue;
        volumeSlider.value = volumeInitialValue * 100f;
        onInputFieldsDone = onDone;

        return this;
    }

    /// <summary>
    /// Closes the settings modal window.
    /// </summary>
    public void CloseSettings()
    {
        Close();
    }

    /// <summary>
    /// Submits the input from the settings modal window.
    /// </summary>
    void SubmitInput()
    {
        onInputFieldsDone?.Invoke(nameField.text, volumeSlider.value);
    }

    /// <summary>
    /// Overrides the Update method to handle input submission and escape key presses.
    /// </summary>
    protected override void Update()
    {
        base.Update();
        if ((nameField.isFocused || volumeSlider.interactable) && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonUp(0)))
            SubmitInput();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return;
        }
    }

    /// <summary>
    /// Handles the event when the OK button of the input field is clicked.
    /// </summary>
    public void UI_InputFieldOKButton()
    {
        SubmitInput();
        CloseSettings();
    }

    /// <summary>
    /// Sets whether the game should be paused when the settings modal window is closed.
    /// </summary>
    /// <param name="shouldPause">True if the game should be paused, false otherwise.</param>
    /// <returns>The current instance of the settings modal window.</returns>
    public SettingsModalWindow SetShouldPauseOnClose(bool shouldPause)
    {
        shouldPauseOnClose = shouldPause;
        return this;
    }

    /// <summary>
    /// Overrides the Close method to handle pausing the game when the window is closed.
    /// </summary>
    /// <returns>The current instance of the settings modal window.</returns>
    public override SettingsModalWindow Close()
    {
        base.Close();

        if (shouldPauseOnClose)
        {
            StartCoroutine(TemporaryUnpauseAndPause());
        }
        else
        {
            Time.timeScale = 1f; // Unpause the game
        }

        return this;
    }

    /// <summary>
    /// Temporarily unpauses the game and then pauses it.
    /// </summary>
    private IEnumerator TemporaryUnpauseAndPause()
    {
        // Unpause the game for 0.3 seconds
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.3f);

        // Pause the game
        Time.timeScale = 0f;
    }
}