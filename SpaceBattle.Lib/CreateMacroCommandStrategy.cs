using Hwdtech;

namespace StarWars.Lib;

public class CreateMacroCommandStrategy
{
    private readonly string _commandSpec;

    public CreateMacroCommandStrategy(string commandSpec)
    {
        _commandSpec = commandSpec;
    }

    public Hwdtech.ICommand Resolve(object[] args)
    {
        var commandNames = IoC.Resolve<IEnumerable<string>>($"Specs.{_commandSpec}", args);
        var commands = commandNames.Select(name => IoC.Resolve<Hwdtech.ICommand>(name, args));
        return new MacroCommand(commands);
    }
}
