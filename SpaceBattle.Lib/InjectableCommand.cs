namespace StarWars.Lib;

public class CommandInjectableCommand : ICommand, ICommandInjectable
{
    private Hwdtech.ICommand? _injectedCommand;

    public CommandInjectableCommand() { }

    public void Inject(Hwdtech.ICommand command)
    {
        _injectedCommand = command;
    }

    public void Execute()
    {
        if (_injectedCommand == null)
        {
            throw new InvalidOperationException("Command not injected.");
        }

        _injectedCommand.Execute();
    }
}
