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

    private readonly Stack<Command> done, undone;

    public History()
    {
        done = new Stack<Command>();
        undone = new Stack<Command>();
    }

    public void Do(Command new_cmd)
    {
        new_cmd.Execute();
        undone.Clear();
        done.Push(new_cmd);
    }

    public bool CanUndo()
    {
        return done.Count > 0;
    }

    public void Undo()
    {
        if (done.Count == 0)
        {
            return;
        }
        Command toUndo = done.Pop();
        toUndo.Unexecute();
        undone.Push(toUndo);
    }

    public bool CanRedo()
    {
        return undone.Count > 0;
    }

    public void Redo()
    {
        if (undone.Count == 0)
        {
            return;
        }
        Command toDo = undone.Pop();
        toDo.Execute();
        done.Push(toDo);
    }

    public void DeleteInstance()
    {
        done.Clear();
        undone.Clear();
        instance = null;
    }
}