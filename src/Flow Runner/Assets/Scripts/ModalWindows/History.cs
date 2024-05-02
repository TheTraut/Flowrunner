using System.Collections.Generic;

class History
{
    /// <summary>
    /// Gets the instance of the History, creating one if none exists.
    /// </summary>
    /// <returns>The instance of History.</returns>
    private static History instance;
    public static History Instance
    {
        get
        {
            instance ??= new History();
            return instance;
        }
    }

    private readonly Stack<Command> undoStack = new();
    private readonly Stack<Command> redoStack = new();

    public Command CurrentCommand { get; private set; }

    public void Do(Command command)
    {
        command.Execute();
        undoStack.Push(command);

        // Update the current command
        CurrentCommand = command;

        // Clear redo stack when a new command is executed
        redoStack.Clear();
    }

    public bool CanUndo()
    {
        return undoStack.Count > 0;
    }

    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            Command command = undoStack.Pop();
            command.Unexecute();
            redoStack.Push(command);

            // Update the current command
            CurrentCommand = command;
        }
    }

    public bool CanRedo()
    {
        return redoStack.Count > 0;
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            Command command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);

            // Update the current command
            CurrentCommand = command;
        }
    }

    public void DeleteInstance()
    {
        undoStack.Clear();
        redoStack.Clear();
        instance = null;
    }
}