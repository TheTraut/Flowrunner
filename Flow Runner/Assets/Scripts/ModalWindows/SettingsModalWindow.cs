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

    public SettingsModalWindow SetSettings(Action<string, string> onDone, string initialValue = "", string volumeInitialValue = "80", string placeholderValue = "Type here", string volumePlaceholderValue = "Type here")
    {
        nameField.text = initialValue;
        ((Text)nameField.placeholder).text = placeholderValue;
        volumeField.text = volumeInitialValue;
        ((Text)volumeField.placeholder).text = volumePlaceholderValue;
        onInputFieldsDone = onDone;

        return this;
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
    }

    public void UI_InputFieldOKButton()
    {
        SubmitInput();
    }
}