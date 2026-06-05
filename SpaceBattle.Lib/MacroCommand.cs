namespace StarWars.Lib;

public class MacroCommand : Hwdtech.ICommand
{
    private readonly IReadOnlyList<Hwdtech.ICommand> _commands;

    public MacroCommand(IEnumerable<Hwdtech.ICommand> commands)
    {
        _commands = commands.ToList();
    }

    public void Execute()
    {
        ExecuteFrom(0);
    }

    private void ExecuteFrom(int index)
    {
        if (index >= _commands.Count)
        {
            return;
        }

        _commands[index].Execute();
        ExecuteFrom(index + 1);
    }
}
