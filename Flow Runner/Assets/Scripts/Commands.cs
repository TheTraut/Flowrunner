using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Command
{
    // Execute method to perform the command's action
    public abstract void Execute();

    // Unexecute method to revert the command's action
    public abstract void Unexecute();
}

// Command to set the name
public class SetNameCommand : Command
{
    public readonly string previousName;
    public readonly string newName;

    public SetNameCommand(string previousName, string newName)
    {
        this.previousName = previousName;
        this.newName = newName;
    }

    public override void Execute()
    {
        SettingsManager.Instance.SetName(newName);
    }

    public override void Unexecute()
    {
        SettingsManager.Instance.SetName(previousName);
    }
}

// Command to set the volume
public class SetVolumeCommand : Command
{
    public readonly float previousVolume;
    public readonly float newVolume;

    public SetVolumeCommand(float previousVolume, float newVolume)
    {
        this.previousVolume = previousVolume;
        this.newVolume = newVolume;
    }

    public override void Execute()
    {
        SettingsManager.Instance.SetVolume(newVolume);
    }

    public override void Unexecute()
    {
        SettingsManager.Instance.SetVolume(previousVolume);
    }
}

// Command to set the shortcut
public class SetShortcutCommand : Command
{
    public readonly InputField inputField;
    public readonly List<KeyCode> previousShortcut;
    public readonly List<KeyCode> newShortcut;

    public SetShortcutCommand(InputField inputField, List<KeyCode> previousShortcut, List<KeyCode> newShortcut)
    {
        this.inputField = inputField;
        this.previousShortcut = previousShortcut;
        this.newShortcut = newShortcut;
    }

    public override void Execute()
    {
        if (inputField == SettingsModalWindow.Instance.upShortcutField)
        {
            SettingsManager.Instance.SetUpShortcut(newShortcut);
        }
        else if (inputField == SettingsModalWindow.Instance.downShortcutField)
        {
            SettingsManager.Instance.SetDownShortcut(newShortcut);
        }
        else if (inputField == SettingsModalWindow.Instance.shieldShortcutField)
        {
            SettingsManager.Instance.SetShieldShortcut(newShortcut);
        }
    }

    public override void Unexecute()
    {
        if (inputField == SettingsModalWindow.Instance.upShortcutField)
        {
            SettingsManager.Instance.SetUpShortcut(previousShortcut);
        }
        else if (inputField == SettingsModalWindow.Instance.downShortcutField)
        {
            SettingsManager.Instance.SetDownShortcut(previousShortcut);
        }
        else if (inputField == SettingsModalWindow.Instance.shieldShortcutField)
        {
            SettingsManager.Instance.SetShieldShortcut(previousShortcut);
        }
    }
}

public class SetSettingsCommand : Command
{
    private readonly string previousName;
    private readonly float previousVolume;
    private readonly List<KeyCode> previousUpKeys;
    private readonly List<KeyCode> previousDownKeys;
    private readonly List<KeyCode> previousShieldKeys;

    private readonly string newName;
    private readonly float newVolume;
    private readonly List<KeyCode> newUpKeys;
    private readonly List<KeyCode> newDownKeys;
    private readonly List<KeyCode> newShieldKeys;

    public SetSettingsCommand(string previousName, float previousVolume, List<KeyCode> previousUpKeys, List<KeyCode> previousDownKeys, List<KeyCode> previousShieldKeys,
                              string newName, float newVolume, List<KeyCode> newUpKeys, List<KeyCode> newDownKeys, List<KeyCode> newShieldKeys)
    {
        this.previousName = previousName;
        this.previousVolume = previousVolume;
        this.previousUpKeys = new(previousUpKeys);
        this.previousDownKeys = new(previousDownKeys);
        this.previousShieldKeys = new(previousShieldKeys);

        this.newName = newName;
        this.newVolume = newVolume;
        this.newUpKeys = new(newUpKeys);
        this.newDownKeys = new(newDownKeys);
        this.newShieldKeys = new(newShieldKeys);
    }

    public override void Execute()
    {
        // Set new settings
        SettingsManager.Instance.SetName(newName);
        SettingsManager.Instance.SetVolume(newVolume);
        SettingsManager.Instance.SetUpShortcut(newUpKeys);
        SettingsManager.Instance.SetDownShortcut(newDownKeys);
        SettingsManager.Instance.SetShieldShortcut(newShieldKeys);
        SettingsManager.Instance.SaveSettings();
    }

    public override void Unexecute()
    {
        // Restore previous settings
        SettingsManager.Instance.SetName(previousName);
        SettingsManager.Instance.SetVolume(previousVolume);
        SettingsManager.Instance.SetUpShortcut(previousUpKeys);
        SettingsManager.Instance.SetDownShortcut(previousDownKeys);
        SettingsManager.Instance.SetShieldShortcut(previousShieldKeys);
        SettingsManager.Instance.SaveSettings();
    }
}

// Command class for deleting a highscore entry
public class DeleteHighscoreCommand : Command
{
    private readonly HighscoreEntry deletedEntry;

    public DeleteHighscoreCommand(HighscoreEntry entry)
    {
        deletedEntry = entry;
    }

    public override void Execute()
    {
        HighscoresManager.Instance.RemoveHighscore(deletedEntry.id);
    }

    public override void Unexecute()
    {
        HighscoresManager.Instance.AddHighscore(deletedEntry.playerName, deletedEntry.score, deletedEntry.id);
    }
}
