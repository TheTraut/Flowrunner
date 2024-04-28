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
    private string nameFieldInitialValue;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private InputField upShortcutField;
    [SerializeField] private InputField downShortcutField;
    [SerializeField] private InputField shieldShortcutField;
    private InputField activeInputField;
    [SerializeField] private Sprite inputSprite;
    [SerializeField] private Sprite inputOutlineRed;
    [SerializeField] private Sprite inputOutlineYellow;
    [SerializeField] private Sprite inputOutlineGreen;

    private Action<string, float, List<KeyCode>, List<KeyCode>, List<KeyCode>> onInputFieldsDone;
    private List<KeyCode> capturedKeys = new List<KeyCode>();
    private bool isCapturing = false;

    private float flashDuration = 0.3f; // Duration for each flash
    private bool isFlashing = false; // Flag to indicate if flashing is in progress

    public SettingsModalWindow SetSettings(Action<string, float, List<KeyCode>, List<KeyCode>, List<KeyCode>> onDone, string initialValue = "", float volumeInitialValue = 0.8f, string placeholderValue = "Type here", List<KeyCode> upShortcut = null, List<KeyCode> downShortcut = null, List<KeyCode> shieldShortcut = null)
    {
        nameField.text = initialValue;
        nameFieldInitialValue = initialValue; // Store the initial value of the name field
        ((Text)nameField.placeholder).text = placeholderValue;
        volumeSlider.value = volumeInitialValue * 100f;

        if (upShortcut != null)
            upShortcutField.text = KeyCodeListToString(upShortcut);
        if (downShortcut != null)
            downShortcutField.text = KeyCodeListToString(downShortcut);
        if (shieldShortcut != null)
            shieldShortcutField.text = KeyCodeListToString(shieldShortcut);

        onInputFieldsDone = onDone;

        return this;
    }

    protected override void Update()
    {
        // Not implementing to disable the 'esc' key detection to close the modal
        //base.Update();

        // Check for text modification for the name field
        if (nameField.text != nameFieldInitialValue)
        {
            SetInputFieldSprite(nameField, inputOutlineYellow);
        }
        else
        {
            // Check if the sprite is not already inputSprite or green, then set it back to inputSprite
            if (nameField.GetComponent<Image>().sprite != inputSprite && nameField.GetComponent<Image>().sprite != inputOutlineGreen)
            {
                SetInputFieldSprite(nameField, inputSprite);
            }
        }

        // Check for the up shortcut field
        CheckAndUpdateShortcutField(upShortcutField);

        // Check for the down shortcut field
        CheckAndUpdateShortcutField(downShortcutField);

        // Check for the shield shortcut field
        CheckAndUpdateShortcutField(shieldShortcutField);

        // Clear input field when focus is brought to it and start capturing
        if (!isCapturing)
        {
            if (upShortcutField.isFocused)
            {
                upShortcutField.text = "";
                StartCapture(upShortcutField);
            }
            else if (downShortcutField.isFocused)
            {
                downShortcutField.text = "";
                StartCapture(downShortcutField);
            }
            else if (shieldShortcutField.isFocused)
            {
                shieldShortcutField.text = "";
                StartCapture(shieldShortcutField);
            }
        }

        if (isCapturing && activeInputField != null)
        {
            UpdateCapturingIndicator(activeInputField);
        }

        if (isCapturing)
        {
            UpdateCapturingIndicator(activeInputField);

            bool escapePressed = Input.GetKeyDown(KeyCode.Escape);
            bool anyKeyReleased = capturedKeys.Exists(key => !Input.GetKey(key)); // Check if any captured key is released

            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                bool keyDown = Input.GetKeyDown(code);

                if (keyDown && !capturedKeys.Contains(code))
                {
                    capturedKeys.Add(code);
                    UpdateShortcutField();
                    break;
                }
                else if (keyDown && capturedKeys.Contains(code))
                {
                    escapePressed = false; // Avoid resetting shortcut if captured key is pressed again
                }
            }

            if (anyKeyReleased && (capturedKeys.Count > 0)) // Check for any key released
            {
                StopCapture();
            }
            else if (escapePressed)
            {
                ResetShortcutField();
                StopCapture();
            }
        }
    }

    void SetInputFieldSprite(InputField field, Sprite sprite)
    {
        Image image = field.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
        }
        else
        {
            Debug.LogError("No Image component found on the InputField GameObject.");
        }
    }

    void StartCapture(InputField inputField)
    {
        capturedKeys.Clear();
        isCapturing = true;
        SetActiveInputField(inputField);
    }

    void StopCapture()
    {
        isCapturing = false;
        ClearActiveInputField();
    }

    void SetActiveInputField(InputField inputField)
    {
        activeInputField = inputField;
    }

    void ClearActiveInputField()
    {
        activeInputField.DeactivateInputField();
        activeInputField = null;
    }

    void UpdateCapturingIndicator(InputField field)
    {
        if (field.text == "")
        {
            SetInputFieldSprite(field, inputOutlineRed); // Set to red outline sprite if text is empty
        }
        else if (isCapturing && field == activeInputField)
        {
            if (!isFlashing)
            {
                SetInputFieldSprite(field, inputOutlineYellow); // Set to yellow outline sprite if capturing keystrokes
                                                                // Start flashing
                StartCoroutine(FlashOutline(field));
            }
        }
        else
        {
            SetInputFieldSprite(field, inputOutlineGreen); // Set to green outline sprite if not capturing and text is not empty
        }
    }

    IEnumerator FlashOutline(InputField field)
    {
        isFlashing = true;
        while (isCapturing && field == activeInputField)
        {
            SetInputFieldSprite(field, inputOutlineYellow); // Set to yellow outline sprite
            yield return new WaitForSeconds(flashDuration);
            SetInputFieldSprite(field, inputSprite); // Set to default sprite
            yield return new WaitForSeconds(flashDuration);
        }
        SetInputFieldSprite(field, inputOutlineYellow);
        isFlashing = false;
    }

    void CheckAndUpdateShortcutField(InputField shortcutField)
    {
        // Check for text modification for the shortcut field
        if (shortcutField.text != GetShortcutFieldInitialValue(shortcutField))
        {
            SetInputFieldSprite(shortcutField, inputOutlineYellow);
        }
        else
        {
            // Check if the sprite is not already inputSprite or green, then set it back to inputSprite
            if (shortcutField.GetComponent<Image>().sprite != inputSprite && shortcutField.GetComponent<Image>().sprite != inputOutlineGreen)
            {
                SetInputFieldSprite(shortcutField, inputSprite);
            }
        }
    }

    string GetShortcutFieldInitialValue(InputField shortcutField)
    {
        if (shortcutField == upShortcutField)
        {
            return KeyCodeListToString(SettingsManager.Instance.UpShortcutKeys);
        }
        else if (shortcutField == downShortcutField)
        {
            return KeyCodeListToString(SettingsManager.Instance.DownShortcutKeys);
        }
        else if (shortcutField == shieldShortcutField)
        {
            return KeyCodeListToString(SettingsManager.Instance.ShieldShortcutKeys);
        }

        return "";
    }

    void UpdateShortcutField()
    {
        if (upShortcutField.isFocused)
        {
            upShortcutField.text = KeyCodeListToString(capturedKeys);
        }
        else if (downShortcutField.isFocused)
        {
            downShortcutField.text = KeyCodeListToString(capturedKeys);
        }
        else if (shieldShortcutField.isFocused)
        {
            shieldShortcutField.text = KeyCodeListToString(capturedKeys);
        }
    }

    void ResetShortcutField()
    {
        if (activeInputField != null)
        {
            string originalValue = "";

            // Retrieve original value based on the active input field
            if (activeInputField == upShortcutField)
            {
                originalValue = KeyCodeListToString(SettingsManager.Instance.UpShortcutKeys);
            }
            else if (activeInputField == downShortcutField)
            {
                originalValue = KeyCodeListToString(SettingsManager.Instance.DownShortcutKeys);
            }
            else if (activeInputField == shieldShortcutField)
            {
                originalValue = KeyCodeListToString(SettingsManager.Instance.ShieldShortcutKeys);
            }

            activeInputField.text = originalValue;
            SetInputFieldSprite(activeInputField, inputSprite);
        }
    }

    public void UI_InputFieldOKButton()
    {
        SaveSettings();
        Close();
    }

    private void SaveSettings()
    {
        List<KeyCode> upKeys = StringToKeyCodeList(upShortcutField.text);
        List<KeyCode> downKeys = StringToKeyCodeList(downShortcutField.text);
        List<KeyCode> shieldKeys = StringToKeyCodeList(shieldShortcutField.text);
        onInputFieldsDone?.Invoke(nameField.text, volumeSlider.value, upKeys, downKeys, shieldKeys);

        UpdateInputFieldSprites();
    }

    private void UpdateInputFieldSprites()
    {
        SetInputFieldSprite(nameField, nameField.text == "" ? inputOutlineRed : inputOutlineGreen);
        SetInputFieldSprite(upShortcutField, upShortcutField.text == "" ? inputOutlineRed : inputOutlineGreen);
        SetInputFieldSprite(downShortcutField, downShortcutField.text == "" ? inputOutlineRed : inputOutlineGreen);
        SetInputFieldSprite(shieldShortcutField, shieldShortcutField.text == "" ? inputOutlineRed : inputOutlineGreen);
    }

    string KeyCodeListToString(List<KeyCode> keyCodes)
    {
        string result = "";
        foreach (KeyCode keyCode in keyCodes)
        {
            if (result != "")
                result += " + ";
            result += keyCode.ToString();
        }
        return result;
    }

    List<KeyCode> StringToKeyCodeList(string input)
    {
        string[] keyStrings = input.Split('+');
        List<KeyCode> keyCodes = new List<KeyCode>();
        foreach (string keyString in keyStrings)
        {
            if (Enum.TryParse(keyString.Trim(), out KeyCode parsedKeyCode))
            {
                keyCodes.Add(parsedKeyCode);
            }
        }
        return keyCodes;
    }
}