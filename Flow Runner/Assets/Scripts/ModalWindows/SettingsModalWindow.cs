using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Manages the settings modal window.
/// </summary>
public class SettingsModalWindow : ModalWindow<SettingsModalWindow>
{
    public static new SettingsModalWindow Instance { get; set; }

    #pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] private InputField nameField;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] public InputField upShortcutField;
    [SerializeField] public InputField downShortcutField;
    [SerializeField] public InputField shieldShortcutField;
    private InputField activeInputField;
    [SerializeField] private Sprite inputSprite;
    [SerializeField] private Sprite inputOutlineRed;
    [SerializeField] private Sprite inputOutlineYellow;
    [SerializeField] private Sprite inputOutlineGreen;
    [SerializeField] private Button undoButton;
    [SerializeField] private Button redoButton;
    #pragma warning restore IDE0044 // Add readonly modifier

    private readonly List<KeyCode> capturedKeys = new();
    private bool isCapturing = false;

    private readonly float FLASH_DURATION = 0.3f; // Duration for each flash
    private bool isFlashing = false; // Flag to indicate if flashing is in progress

    private string initialName;
    // Define a dictionary to store previous shortcuts for each InputField
    private readonly Dictionary<InputField, List<KeyCode>> initialShortcuts = new();

    private string previousName;
    private float previousVolume;
    // Define a dictionary to store previous shortcuts for each InputField
    private readonly Dictionary<InputField, List<KeyCode>> previousShortcuts = new();

    public SettingsModalWindow SetSettings(string initialValue = "", float volumeInitialValue = 0.8f, string placeholderValue = "Type here", List<KeyCode> upShortcut = null, List<KeyCode> downShortcut = null, List<KeyCode> shieldShortcut = null)
    {
        nameField.text = initialValue;
        ((Text)nameField.placeholder).text = placeholderValue;
        volumeSlider.value = volumeInitialValue * 100f;

        if (upShortcut != null)
        {
            upShortcutField.text = KeyCodeListToString(upShortcut);
        }
        if (downShortcut != null)
        {
            downShortcutField.text = KeyCodeListToString(downShortcut);
        }
        if (shieldShortcut != null)
        {
            shieldShortcutField.text = KeyCodeListToString(shieldShortcut);
        }

        return this;
    }

    private void Awake()
    {
        Instance = this;

        SettingsManager.Instance.LoadSettings();
        UpdateUndoRedoButtonVisibility();

        undoButton.onClick.AddListener(() =>
        {
            History.Instance.Undo();
            UpdateUndoRedoButtonVisibility();
            UpdateInputs(true);
        });
        redoButton.onClick.AddListener(() =>
        {
            History.Instance.Redo();
            UpdateUndoRedoButtonVisibility();
            UpdateInputs(false);
        });

        // Add event listeners for input field changes
        nameField.onEndEdit.AddListener(OnNameFieldEndEdit);

        // Add EventTrigger component to the volume slider
        EventTrigger volumeSliderPointerUpTrigger = volumeSlider.gameObject.AddComponent<EventTrigger>();
        // Create a pointer up event trigger
        EventTrigger.Entry pointerUpEntry = new()
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener((eventData) => { OnVolumeSliderValueChanged(volumeSlider.value); });
        // Add the pointer up event trigger to the volume slider
        volumeSliderPointerUpTrigger.triggers.Add(pointerUpEntry);

        // Focus events to set previous values
        EventTrigger.Entry selectEntry = new()
        {
            eventID = EventTriggerType.Select
        };
        selectEntry.callback.AddListener((eventData) => { OnUIElementSelect(eventData); });
        AddSelectEventTrigger(nameField.gameObject, selectEntry);
        AddSelectEventTrigger(volumeSlider.gameObject, selectEntry);
        AddSelectEventTrigger(upShortcutField.gameObject, selectEntry);
        AddSelectEventTrigger(downShortcutField.gameObject, selectEntry);
        AddSelectEventTrigger(shieldShortcutField.gameObject, selectEntry);
    }

    private void Start()
    {
        // Store initial values
        StoreInitialValues();

        // Initialize previous shortcuts dictionary with copies of initial shortcuts
        previousShortcuts[upShortcutField] = new List<KeyCode>(initialShortcuts[upShortcutField]);
        previousShortcuts[downShortcutField] = new List<KeyCode>(initialShortcuts[downShortcutField]);
        previousShortcuts[shieldShortcutField] = new List<KeyCode>(initialShortcuts[shieldShortcutField]);
    }

    public override SettingsModalWindow Close()
    {
        Instance = null;
        base.Close();
        return Instance;
    }

    private void CloseSettings()
    {
        History.Instance.DeleteInstance();
        SettingsManager.Instance.LoadSettings();
        Close();
    }

    private void StoreInitialValues()
    {
        initialName = nameField.text;
        //initialVolume = volumeSlider.value;
        initialShortcuts[upShortcutField] = SettingsManagerExtensions.StringToKeyCodeList(upShortcutField.text);
        initialShortcuts[downShortcutField] = SettingsManagerExtensions.StringToKeyCodeList(downShortcutField.text);
        initialShortcuts[shieldShortcutField] = SettingsManagerExtensions.StringToKeyCodeList(shieldShortcutField.text);
    }

    private void AddSelectEventTrigger(GameObject gameObject, EventTrigger.Entry selectEntry)
    {
        EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();
        eventTrigger.triggers.Add(selectEntry);
    }

    // Function to handle select events for input fields and slider
    private void OnUIElementSelect(BaseEventData eventData)
    {
        GameObject selectedGameObject = eventData.selectedObject;
        if (selectedGameObject != null)
        {
            if (selectedGameObject.TryGetComponent(out InputField selectedInputField))
            {
                if (selectedInputField == nameField)
                {
                    previousName = selectedInputField.text;
                }
                else if (selectedInputField == upShortcutField || selectedInputField == downShortcutField || selectedInputField == shieldShortcutField)
                {
                    previousShortcuts[selectedInputField] = SettingsManagerExtensions.StringToKeyCodeList(selectedInputField.text);
                }
            }
            else if (selectedGameObject.TryGetComponent(out Slider selectedSlider))
            {
                if (selectedSlider == volumeSlider)
                {
                    previousVolume = selectedSlider.value;
                }
            }
        }
    }

    private void UpdateInputs(bool usePreviousValues)
    {
        // Retrieve the command instance from the history
        Command currentCommand = History.Instance.CurrentCommand;

        if (currentCommand is SetNameCommand setNameCommand)
        {
            // Set name field to previous or new value depending on the parameter
            nameField.text = usePreviousValues ? setNameCommand.previousName : setNameCommand.newName;
        }

        if (currentCommand is SetVolumeCommand setVolumeCommand)
        {
            // Set volume slider to previous or new value depending on the parameter
            volumeSlider.value = usePreviousValues ? setVolumeCommand.previousVolume : setVolumeCommand.newVolume;
        }

        if (currentCommand is SetShortcutCommand setShortcutCommand)
        {
            // Set up shortcut field to previous or new value depending on the parameter
            if (setShortcutCommand.inputField == upShortcutField)
            {
                upShortcutField.text = usePreviousValues ? KeyCodeListToString(setShortcutCommand.previousShortcut) : KeyCodeListToString(setShortcutCommand.newShortcut);
            }
            // Set down shortcut field to previous or new value depending on the parameter
            else if (setShortcutCommand.inputField == downShortcutField)
            {
                downShortcutField.text = usePreviousValues ? KeyCodeListToString(setShortcutCommand.previousShortcut) : KeyCodeListToString(setShortcutCommand.newShortcut);
            }
            // Set shield shortcut field to previous or new value depending on the parameter
            else if (setShortcutCommand.inputField == shieldShortcutField)
            {
                shieldShortcutField.text = usePreviousValues ? KeyCodeListToString(setShortcutCommand.previousShortcut) : KeyCodeListToString(setShortcutCommand.newShortcut);
            }
        }
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

    private void OnNameFieldEndEdit(string newName)
    {
        if (nameField.text != previousName)
        {
            // Store the change in history
            Command setNameCommand = new SetNameCommand(previousName, newName);
            History.Instance.Do(setNameCommand);
            UpdateUndoRedoButtonVisibility();
            previousName = newName;
        }
    }

    private void OnVolumeSliderValueChanged(float newVolume)
    {
        if (newVolume != previousVolume)
        {
            // Store the change in history
            Command setVolumeCommand = new SetVolumeCommand(previousVolume, newVolume);
            History.Instance.Do(setVolumeCommand);
            UpdateUndoRedoButtonVisibility();
            previousVolume = newVolume;
        }
    }

    private void UpdateShortcutField(string newShortcut)
    {
        List<KeyCode> parsedShortcut = SettingsManagerExtensions.StringToKeyCodeList(newShortcut);
        List<KeyCode> previousShortcut;
        try
        {
            previousShortcut = previousShortcuts[activeInputField];
            if (previousShortcut != null && !parsedShortcut.SequenceEqual(previousShortcut))
            {
                Command setShortcutCommand = new SetShortcutCommand(activeInputField, previousShortcut, parsedShortcut);
                History.Instance.Do(setShortcutCommand);
                UpdateUndoRedoButtonVisibility();
                previousShortcuts[activeInputField] = parsedShortcut;
            }
        }
        catch (KeyNotFoundException)
        {
            Destroy(gameObject);
        }
    }

    protected override void Update()
    {
        // Not implementing to disable the 'esc' key detection to close the modal
        //base.Update();

        // Check for text modification for the name field
        if (nameField.text != initialName)
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
            else if (escapePressed || !activeInputField.isFocused)
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
        UpdateShortcutField(activeInputField.text);
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
                StartCoroutine(FlashOutline(field)); // Start flashing to indicate capturing
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
            SetInputFieldSprite(field, inputOutlineRed); // Set to red outline sprite
            yield return new WaitForSeconds(FLASH_DURATION);
            SetInputFieldSprite(field, inputSprite); // Set to default sprite
            yield return new WaitForSeconds(FLASH_DURATION);
        }
        SetInputFieldSprite(field, inputOutlineRed);
        isFlashing = false;
    }

    void CheckAndUpdateShortcutField(InputField shortcutField)
    {
        // Check for text modification for the shortcut field
        if (!isCapturing && shortcutField.text != GetShortcutFieldInitialValue(shortcutField))
        {
            SetInputFieldSprite(shortcutField, inputOutlineYellow);
        }
        else if (!isCapturing && shortcutField.GetComponent<Image>().sprite != inputSprite && shortcutField.GetComponent<Image>().sprite != inputOutlineGreen)
        {
            SetInputFieldSprite(shortcutField, inputSprite);
        }
    }

    string GetShortcutFieldInitialValue(InputField shortcutField)
    {
        try
        {
            if (shortcutField == upShortcutField || shortcutField == downShortcutField || shortcutField == shieldShortcutField)
            {
                return KeyCodeListToString(initialShortcuts[shortcutField]);
            }
        }
        catch (KeyNotFoundException)
        {
            Destroy(gameObject);
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
            try
            {
                if (activeInputField == upShortcutField || activeInputField == downShortcutField || activeInputField == shieldShortcutField)
                {
                    originalValue = KeyCodeListToString(initialShortcuts[activeInputField]);
                }
            }
            catch (KeyNotFoundException)
            {
                Destroy(gameObject);
            }
            activeInputField.text = originalValue;
            SetInputFieldSprite(activeInputField, inputSprite);
        }
    }

    public void UI_InputFieldOKButton()
    {
        SaveSettings();
        CloseSettings();
    }

    private void SaveSettings()
    {
        // Extract new settings values from UI components
        string name = nameField.text;
        float volume = volumeSlider.value;
        List<KeyCode> upKeys = SettingsManagerExtensions.StringToKeyCodeList(upShortcutField.text);
        List<KeyCode> downKeys = SettingsManagerExtensions.StringToKeyCodeList(downShortcutField.text);
        List<KeyCode> shieldKeys = SettingsManagerExtensions.StringToKeyCodeList(shieldShortcutField.text);

        // Create and execute command to set settings
        Command setSettingsCommand = new SetSettingsCommand(previousName, previousVolume, previousShortcuts[upShortcutField], previousShortcuts[downShortcutField], previousShortcuts[shieldShortcutField], name, volume, upKeys, downKeys, shieldKeys);
        History.Instance.Do(setSettingsCommand);
        UpdateUndoRedoButtonVisibility();

        // Update previous settings variables
        previousName = name;
        previousVolume = volume;
        previousShortcuts[upShortcutField] = upKeys.ToList();
        previousShortcuts[downShortcutField] = downKeys.ToList();
        previousShortcuts[shieldShortcutField] = shieldKeys.ToList();

        UpdateInputFieldSprites();
    }

    private void UpdateInputFieldSprites()
    {
        SetInputFieldSprite(nameField, nameField.text == "" ? inputOutlineRed : inputOutlineGreen);
        SetInputFieldSprite(upShortcutField, upShortcutField.text == "" ? inputOutlineRed : inputOutlineGreen);
        SetInputFieldSprite(downShortcutField, downShortcutField.text == "" ? inputOutlineRed : inputOutlineGreen);
        SetInputFieldSprite(shieldShortcutField, shieldShortcutField.text == "" ? inputOutlineRed : inputOutlineGreen);
    }

    public string KeyCodeListToString(List<KeyCode> keyCodes)
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
}