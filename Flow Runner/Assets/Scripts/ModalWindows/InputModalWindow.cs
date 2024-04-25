using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages a modal window with an input field.
/// </summary>
public class InputModalWindow : ModalWindow<InputModalWindow>
{
    [SerializeField] private InputField inputField;

    private Action<string> onInputFieldDone;

    /// <summary>
    /// Sets up the input field with initial and placeholder values, and specifies the action to be performed when input is done.
    /// </summary>
    /// <param name="onDone">The action to be performed when input is done.</param>
    /// <param name="initialValue">The initial value of the input field.</param>
    /// <param name="placeholderValue">The placeholder value of the input field.</param>
    /// <returns>The current instance of the input modal window.</returns>
    public InputModalWindow SetInputField(Action<string> onDone, string initialValue = "", string placeholderValue = "Type here")
    {
        inputField.text = initialValue;
        ((Text)inputField.placeholder).text = placeholderValue;
        onInputFieldDone = onDone;

        return this;
    }

    /// <summary>
    /// Handles actions to be performed before the modal window is shown.
    /// </summary>
    protected override void OnBeforeShow()
    {
        inputField.Select();
    }

    /// <summary>
    /// Handles actions to be performed when the input is submitted.
    /// </summary>
    void SubmitInput()
    {
        onInputFieldDone?.Invoke(inputField.text);
        onInputFieldDone = null;
        Close();
    }

    /// <summary>
    /// Updates the modal window.
    /// </summary>
    protected override void Update()
    {
        base.Update();
        if (inputField.isFocused && inputField.text != "" && Input.GetKeyUp(KeyCode.Return))
            SubmitInput();
    }

    /// <summary>
    /// Handles the event when the OK button of the input field is clicked.
    /// </summary>
    public void UI_InputFieldOKButton()
    {
        SubmitInput();
    }
}
