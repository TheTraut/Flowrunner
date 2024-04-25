using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsModalWindow : ModalWindow<SettingsModalWindow>
{
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField volumeField;

    private Action<string, string> onInputFieldsDone;
    private bool shouldPauseOnClose = true;

    public SettingsModalWindow SetSettings(Action<string, string> onDone, string initialValue = "", string volumeInitialValue = "80", string placeholderValue = "Type here", string volumePlaceholderValue = "Type here")
    {
        nameField.text = initialValue;
        ((Text)nameField.placeholder).text = placeholderValue;
        volumeField.text = volumeInitialValue;
        ((Text)volumeField.placeholder).text = volumePlaceholderValue;
        onInputFieldsDone = onDone;

        return this;
    }

    public void CloseSettings()
    {
        Close();
    }

    void SubmitInput()
    {
        onInputFieldsDone?.Invoke(nameField.text, volumeField.text);
        onInputFieldsDone = null;
        Close();
    }

    protected override void Update()
    {
        base.Update();
        if ((nameField.isFocused || volumeField.isFocused) && Input.GetKeyDown(KeyCode.Return))
            SubmitInput();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return;
        }
    }

    public void UI_InputFieldOKButton()
    {
        SubmitInput();
    }

    public SettingsModalWindow SetShouldPauseOnClose(bool shouldPause)
    {
        shouldPauseOnClose = shouldPause;
        return this;
    }

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

    private IEnumerator TemporaryUnpauseAndPause()
    {
        // Unpause the game for 0.3 seconds
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.3f);

        // Pause the game
        Time.timeScale = 0f;
    }
}