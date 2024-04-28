// Abstract command class
public abstract class Command
{
    // Execute method to perform the command's action
    public abstract void Execute();

    // Unexecute method to revert the command's action
    public abstract void Unexecute();
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
