using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsModalWindow : ModalWindow<SettingsModalWindow>
{
    [SerializeField] private InputField nameField;
    [SerializeField] private Slider volumeSlider;

    private Action<string, float> onInputFieldsDone;
    private bool shouldPauseOnClose = true;

    public SettingsModalWindow SetSettings(Action<string, float> onDone, string initialValue = "", float volumeInitialValue = 0.8f, string placeholderValue = "Type here")
    {
        nameField.text = initialValue;
        ((Text)nameField.placeholder).text = placeholderValue;
        volumeSlider.value = volumeInitialValue * 100f;
        onInputFieldsDone = onDone;

        return this;
    }

    public void CloseSettings()
    {
        Close();
    }

    void SubmitInput()
    {
        onInputFieldsDone?.Invoke(nameField.text, volumeSlider.value);
    }

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

    public void UI_InputFieldOKButton()
    {
        SubmitInput();
        CloseSettings();
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